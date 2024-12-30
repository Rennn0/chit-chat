namespace database.entities
{
    public class Room : Entity
    {
        public Room(string name, string hostUserId, string description, List<string>? users = null)
        {
            Name = name;
            HostUserId = hostUserId;
            Description = description;
            Users = users ?? [];
        }

        public string Name { get; set; }
        public string HostUserId { get; set; }
        public string Description { get; set; }
        public List<string> Users { get; set; }
    }
}
