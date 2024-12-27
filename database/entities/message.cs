using MongoDB.Bson.Serialization.Attributes;

namespace database.entities
{
    public class Message
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public required string RoomId { get; set; }
        public required string UserId { get; set; }
        public required string Context { get; set; }
        public DateTime Timestamp = DateTime.UtcNow;
    }
}
