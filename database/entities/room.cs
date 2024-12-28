namespace database.entities
{
    public class Room : Entity
    {
        public Room(string name, string hostUserId, List<string>? users = null)
        {
            Name = name;
            HostUserId = hostUserId;
            Users = users ?? [];
        }

        public string Name { get; set; }
        public string HostUserId { get; set; }
        public List<string> Users { get; set; }
    }
}
