#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using MongoDB.Bson.Serialization.Attributes;

namespace database.entities;

// TODO sheileba amtvirtavis vinaoba shevinaxo? ideashi zip ukve otaxs ekutvnis da ara users
public class Attachment
{
    [BsonId]
    public string Id { get; set; }
    public string RoomId { get; set; }
    public string FriendlyName { get; set; }
    public ulong Size { get; set; }
}
