using System.Collections.Concurrent;
using database.entities;
using database.interfaces;
using database.mongo;
using Grpc.Core;
using gRpcProtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Message = database.entities.Message;

namespace messageServer.protoServices
{
    public class MessageService : MessageExchangeService.MessageExchangeServiceBase
    {
        private readonly IDatabaseAdapter<User> _userDb;
        private readonly IDatabaseAdapter<Room> _roomDb;
        private readonly IDatabaseAdapter<Message> _messageDb;

        private static readonly ConcurrentDictionary<
            string,
            IServerStreamWriter<gRpcProtos.Message>
        > _dic = new();

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
            await foreach (gRpcProtos.Message? request in requestStream.ReadAllAsync())
            {
                try
                {
                    _dic.TryAdd(request.AuthorUserId, responseStream);

                    await _messageDb.CreateAsync(
                        new Message(request.RoomId, request.AuthorUserId, request.Context)
                    );

                    FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(
                        r => r.Id,
                        request.RoomId
                    );

                    UpdateDefinition<Room> update = Builders<Room>.Update.AddToSet(
                        r => r.Users,
                        request.AuthorUserId
                    );

                    await _roomDb.UpdateAsync(
                        r => r.Id == request.RoomId,
                        new Dictionary<string, object>() { ["Users"] = request.AuthorUserId }
                    );

                    await NotifyNewMessageInRoom(request.RoomId, request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

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

        private async Task NotifyNewMessageInRoom(string roomId, gRpcProtos.Message message)
        {
            Console.WriteLine($"{_dic.Count}");
            foreach (KeyValuePair<string, IServerStreamWriter<gRpcProtos.Message>> kvp in _dic)
            {
                Console.WriteLine($"Sender _ {message.AuthorUserId}, UserId _ {kvp.Key}");

                try
                {
                    await kvp.Value.WriteAsync(message);
                }
                catch { }
            }
        }
    }
}
