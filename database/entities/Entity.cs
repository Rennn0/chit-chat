using MongoDB.Bson.Serialization.Attributes;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace database.entities;

public class Entity
{
    [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
}
