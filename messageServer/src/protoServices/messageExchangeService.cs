using System.Collections.Concurrent;
using database.entities;
using database.interfaces;
using Grpc.Core;
using gRpcProtos;
using Message = database.entities.Message;

namespace messageServer.protoServices
{
    public class MessageService : MessageExchangeService.MessageExchangeServiceBase
    {
        private readonly IDatabaseAdapter<User> _userDb;
        private readonly IDatabaseAdapter<Room> _roomDb;
        private readonly IDatabaseAdapter<Message> _messageDb;

        /// <summary>
        ///     ოთახი იქმენა კონკრეტული იდ_ით, ოთახში ემატებიან კლიენტები თავიანთი იდ_ით +
        ///     ნაკადი სადაც სერვერიდან წასული მესიჯები მიუვათ
        /// </summary>
        private static readonly ConcurrentDictionary<
            string,
            LinkedList<(string userId, IServerStreamWriter<gRpcProtos.Message> stream)>
        > _rooms = new();

        public MessageService(
            IDatabaseAdapter<User> userDb,
            IDatabaseAdapter<Room> roomDb,
            IDatabaseAdapter<Message> messageDb
        )
        {
            _userDb = userDb;
            _roomDb = roomDb;
            _messageDb = messageDb;
        }

        public override async Task MessageStreaming(
            IAsyncStreamReader<gRpcProtos.Message> requestStream,
            IServerStreamWriter<gRpcProtos.Message> responseStream,
            ServerCallContext context
        )
        {
            await foreach (gRpcProtos.Message? request in requestStream.ReadAllAsync()) { }
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

            if (existingUser is not null)
            {
                return existingUser.VerifyPassword(request.Password)
                    ? new CreateUserResponse()
                    {
                        Code = CreateUserResponse.Types.CODE.Created,
                        UserId = existingUser.Id,
                    }
                    : new CreateUserResponse()
                    {
                        Code = CreateUserResponse.Types.CODE.UsernameUsed,
                    };
            }

            User user = new User(request.Password, request.Username);
            await _userDb.CreateAsync(user);
            return new CreateUserResponse
            {
                UserId = user.Id,
                Code = CreateUserResponse.Types.CODE.Created,
            };
        }

        public override async Task<CreateRoomResponse> CreateRoom(
            CreateRoomRequest request,
            ServerCallContext context
        )
        {
            Room room = new Room(request.Name, request.HostUserId);
            await _roomDb.CreateAsync(room);
            return new CreateRoomResponse { RoomId = room.Id };
        }

        private async Task NotifyNewMessageInRoom(string roomId, gRpcProtos.Message message) { }
    }
}
