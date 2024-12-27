using System.Collections.Concurrent;
using System.Formats.Asn1;
using database.entities;
using database.mongo;
using Grpc.Core;
using gRpcProtos;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace messageServer.src.protoServices
{
    public class MessageService : MessageExchangeService.MessageExchangeServiceBase
    {
        private readonly MongoDbContext _dbContext;

        private static ConcurrentDictionary<string, IServerStreamWriter<gRpcProtos.Message>> _dic =
            new();

        public MessageService(IOptions<MongoDbSettings> mongoOptions)
        {
            _dbContext = new MongoDbContext(
                mongoOptions.Value.ConnectionString,
                mongoOptions.Value.Database
            );
        }

        public override async Task MessageStreaming(
            IAsyncStreamReader<gRpcProtos.Message> requestStream,
            IServerStreamWriter<gRpcProtos.Message> responseStream,
            ServerCallContext context
        )
        {
            IClientSessionHandle session = await _dbContext.SessionAsync;

            await foreach (gRpcProtos.Message? request in requestStream.ReadAllAsync())
            {
                try
                {
                    if (!_dic.ContainsKey(request.UserId))
                    {
                        _dic[request.UserId] = responseStream;
                    }

                    await _dbContext.Messages.InsertOneAsync(
                        new database.entities.Message
                        {
                            Context = request.Context,
                            RoomId = request.RoomId,
                            UserId = request.UserId,
                        }
                    );

                    FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(
                        r => r.Id,
                        request.RoomId
                    );

                    UpdateDefinition<Room> update = Builders<Room>.Update.AddToSet(
                        r => r.Users,
                        request.UserId
                    );

                    await _dbContext.Rooms.UpdateOneAsync(
                        filter,
                        update,
                        options: new UpdateOptions { IsUpsert = true }
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
            User user = new User { Email = request.Email, Username = request.Username };
            await _dbContext.Users.InsertOneAsync(user);
            return new CreateUserResponse { UserId = user.Id };
        }

        public override async Task<CreateRoomResponse> CreateRoom(
            CreateRoomRequest request,
            ServerCallContext context
        )
        {
            Room room = new Room { Name = request.Name };
            await _dbContext.Rooms.InsertOneAsync(room);
            return new CreateRoomResponse { RoomId = room.Id };
        }

        private async Task NotifyNewMessageInRoom(string roomId, gRpcProtos.Message message)
        {
            Console.WriteLine($"{_dic.Count}");
            foreach (KeyValuePair<string, IServerStreamWriter<gRpcProtos.Message>> kvp in _dic)
            {
                Console.WriteLine($"Sender _ {message.UserId}, UserId _ {kvp.Key}");

                try
                {
                    await kvp.Value.WriteAsync(message);
                }
                catch { }
            }
        }
    }
}
