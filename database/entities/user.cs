﻿using MongoDB.Bson.Serialization.Attributes;

namespace database.entities
{
    public class User
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public required string Email { get; set; }
        public required string Username { get; set; }
    }
}