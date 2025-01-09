using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using database.entities;
using database.interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using gRpcProtos;
using messageServer.extensions;
using messageServer.rabbit;
using Newtonsoft.Json;
using Message = database.entities.Message;
using RoomTransferObject = llibrary.SharedObjects.Room.RoomTransferObject;

// TODO klientis disconect dasacheria
namespace messageServer.protoServices
{
    public class MessageExchange : MessageExchangeService.MessageExchangeServiceBase
    {
        private readonly IDatabaseAdapter<User> _userDb;
        private readonly IDatabaseAdapter<Room> _roomDb;
        private readonly IDatabaseAdapter<Message> _messageDb;

        /// <summary>
        ///     ოთახი იქმენა კონკრეტული იდ_ით, ოთახში ემატებიან კლიენტები თავიანთი იდ_ით +
        ///     ნაკადი სადაც სერვერიდან წასული მესიჯები მიუვათ
        /// </summary>
        private static ConcurrentDictionary<
            string,
            HashSet<(string userId, IServerStreamWriter<gRpcProtos.Message> stream)>
        > Rooms { get; } = new();

        public MessageExchange(
            IDatabaseAdapter<User> userDb,
            IDatabaseAdapter<Room> roomDb,
            IDatabaseAdapter<Message> messageDb
        )
        {
            _userDb = userDb;
            _roomDb = roomDb;
            _messageDb = messageDb;
        }

        public override async Task<PreloadMessageResponse> PreloadMessages(
            PreloadMessagesRequest request,
            ServerCallContext context
        )
        {
            IEnumerable<Message> list = await _messageDb.GetByAsync(m =>
                m.RoomId == request.RoomId
            );
            PreloadMessageResponse response = new();
            response.Messages.AddRange(
                list.Select(x => new PreloadMessageListObject
                {
                    Context = x.Context,
                    Timestamp = x.Timestamp.ToTimestamp(),
                    UserId = x.AuthorUserId,
                })
            );

            return response;
        }

        /// <summary>
        ///     მესიჯების გაცვლა სერვერსა ყველა კლიენტს შორის
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task MessageStreaming(
            IAsyncStreamReader<gRpcProtos.Message> requestStream,
            IServerStreamWriter<gRpcProtos.Message> responseStream,
            ServerCallContext context
        )
        {
            await foreach (gRpcProtos.Message? request in requestStream.ReadAllAsync())
            {
                UpdateDictionary(request, responseStream);

                if (string.IsNullOrWhiteSpace(request.Context))
                    continue;

                await UpdateDatabase(request);

                await NotifyNewMessageInRoom(request);
            }
        }

        private async Task UpdateDatabase(gRpcProtos.Message request)
        {
            Message newMessage = new Message(request.RoomId, request.AuthorUserId, request.Context);
            await _messageDb.CreateAsync(newMessage);

            int updated = await _roomDb.UpdateAsync(
                r => r.Id == request.RoomId,
                new Dictionary<string, object>() { ["Users"] = newMessage.AuthorUserId }
            );

            request.Timestamp = newMessage.Timestamp.ToTimestamp();
        }

        // TODO strimebis lifecycle sakontroloa, connect / disconnect
        private static void UpdateDictionary(
            gRpcProtos.Message request,
            IServerStreamWriter<gRpcProtos.Message> responseStream
        )
        {
            if (
                !Rooms.TryGetValue(
                    request.RoomId,
                    out HashSet<(
                        string userId,
                        IServerStreamWriter<gRpcProtos.Message> stream
                    )>? hashSet
                )
            )
            {
                Rooms[request.RoomId] = new HashSet<(
                    string userId,
                    IServerStreamWriter<gRpcProtos.Message> stream
                )>(new ClientComparer());
            }

            bool added = Rooms[request.RoomId].Add((request.AuthorUserId, responseStream));
        }

        /// <summary>
        ///     სახელით უნიკალურია კლიენტი.
        ///     პაროლის არასწორად შეყვანისას არ ვეუბნები რომ შეეშალა,
        ///     ითველბა რომ სახელი გამოყენებულია და მორჩა.
        ///     ახალი სახელის დროს პირდაპირ ანგარიში იქმნება.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CreateUserResponse> CreateUser(
            CreateUserRequest request,
            ServerCallContext context
        )
        {
            User? existingUser = (
                await _userDb.GetByAsync(u => u.Username == request.Username.Trim('\r', '\n', ' '))
            ).FirstOrDefault();

            if (existingUser is null)
            {
                return await CreateNewUser(request);
            }

            return VerifyUserRequest(request, existingUser);
        }

        /// <summary>
        ///     ახალი ჰოსტი
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CreateRoomResponse> CreateRoom(
            CreateRoomRequest request,
            ServerCallContext context
        )
        {
            Room room = request.Map();

            await _roomDb.CreateAsync(room);

            RoomTransferObject rto = room.Map();

            string rtoString = JsonConvert.SerializeObject(rto);

            RabbitRoomPublisher.Messages.TryAdd(rtoString);

            return new CreateRoomResponse { RoomId = room.Id };
        }

        private CreateUserResponse VerifyUserRequest(CreateUserRequest request, User existingUser)
        {
            return existingUser.VerifyPassword(request.Password)
                ? new CreateUserResponse()
                {
                    Code = CreateUserResponse.Types.CODE.Created,
                    UserId = existingUser.Id,
                }
                : new CreateUserResponse() { Code = CreateUserResponse.Types.CODE.UsernameUsed };
        }

        private async Task<CreateUserResponse> CreateNewUser(CreateUserRequest request)
        {
            User user = new User(request.Password, request.Username);
            await _userDb.CreateAsync(user);
            return new CreateUserResponse
            {
                UserId = user.Id,
                Code = CreateUserResponse.Types.CODE.Created,
            };
        }

        private async Task NotifyNewMessageInRoom(gRpcProtos.Message message)
        {
            if (
                Rooms.TryGetValue(
                    message.RoomId,
                    out HashSet<(
                        string userId,
                        IServerStreamWriter<gRpcProtos.Message> stream
                    )>? roomClients
                )
            )
            {
                // TODO strimi chaweramde shesamowmebelia
                foreach (
                    (
                        string? userId,
                        IServerStreamWriter<gRpcProtos.Message>? serverStreamWriter
                    ) in roomClients
                )
                {
                    await serverStreamWriter.WriteAsync(message);
                }
            }
        }

        private class ClientComparer
            : IEqualityComparer<(string userId, IServerStreamWriter<gRpcProtos.Message> stream)>
        {
            public bool Equals(
                (string userId, IServerStreamWriter<gRpcProtos.Message> stream) x,
                (string userId, IServerStreamWriter<gRpcProtos.Message> stream) y
            )
            {
                return x.userId == y.userId;
            }

            public int GetHashCode(
                [DisallowNull] (string userId, IServerStreamWriter<gRpcProtos.Message> stream) obj
            )
            {
                return obj.userId.GetHashCode();
            }
        }
    }
}
