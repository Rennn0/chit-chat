namespace database.entities
{
    public class Message : Entity
    {
        public Message(string roomId, string authorUserId, string context)
        {
            Timestamp = DateTime.UtcNow;
            RoomId = roomId;
            AuthorUserId = authorUserId;
            Context = context;
        }

        public string RoomId { get; set; }
        public string AuthorUserId { get; set; }
        public string Context { get; set; }
        public DateTime Timestamp;
    }
}
