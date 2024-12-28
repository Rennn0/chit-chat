using database.entities;
using MongoDB.Driver;

namespace database.mongo
{
    public class MongoDbContext
    {
        public readonly IMongoDatabase Database;
        internal readonly MongoClient _client;

        public MongoDbContext(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            Database = _client.GetDatabase(databaseName);

            EnsureCollectionExists(nameof(Room));
            EnsureCollectionExists(nameof(User));
            EnsureCollectionExists(nameof(Message));

            CreateIndexes();
        }

        public IMongoCollection<Room> Rooms => Database.GetCollection<Room>(nameof(Room));
        public IMongoCollection<User> Users => Database.GetCollection<User>(nameof(User));
        public IMongoCollection<Message> Messages =>
            Database.GetCollection<Message>(nameof(Message));

        private void EnsureCollectionExists(string collectionName)
        {
            if (!Database.ListCollectionNames().ToList().Contains(collectionName))
                Database.CreateCollection(collectionName);
        }

        private void CreateIndexes()
        {
            IMongoCollection<Message> messageCollection = Database.GetCollection<Message>(
                nameof(Message)
            );
            messageCollection.Indexes.CreateMany(
                [
                    new CreateIndexModel<Message>(
                        Builders<Message>
                            .IndexKeys.Ascending(m => m.RoomId)
                            .Ascending(m => m.AuthorUserId)
                    ),
                    new CreateIndexModel<Message>(
                        Builders<Message>.IndexKeys.Hashed(m => m.AuthorUserId)
                    ),
                ]
            );

            IMongoCollection<User> userCollection = Database.GetCollection<User>(nameof(User));
            userCollection.Indexes.CreateOne(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(u => u.Username),
                    new CreateIndexOptions { Unique = true }
                )
            );
        }
    }
}
