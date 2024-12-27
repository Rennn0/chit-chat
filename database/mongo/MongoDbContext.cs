using database.entities;
using MongoDB.Driver;

namespace database.mongo
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoDbContext(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);

            EnsureCollectionExists(nameof(Room));
            EnsureCollectionExists(nameof(User));
            EnsureCollectionExists(nameof(Message));

            CreateIndexes();
        }

        public Task<IClientSessionHandle> SessionAsync => _client.StartSessionAsync();

        public IMongoCollection<Room> Rooms => _database.GetCollection<Room>(nameof(Room));
        public IMongoCollection<User> Users => _database.GetCollection<User>(nameof(User));
        public IMongoCollection<Message> Messages =>
            _database.GetCollection<Message>(nameof(Message));

        private void EnsureCollectionExists(string collectionName)
        {
            if (!_database.ListCollectionNames().ToList().Contains(collectionName))
                _database.CreateCollection(collectionName);
        }

        private void CreateIndexes()
        {
            IMongoCollection<Message> messageCollection = _database.GetCollection<Message>(
                nameof(Message)
            );
            messageCollection.Indexes.CreateMany(
                [
                    new CreateIndexModel<Message>(
                        Builders<Message>
                            .IndexKeys.Ascending(m => m.RoomId)
                            .Ascending(m => m.UserId)
                    ),
                    new CreateIndexModel<Message>(
                        Builders<Message>.IndexKeys.Hashed(m => m.UserId)
                    ),
                ]
            );
        }
    }
}
