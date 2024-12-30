namespace llibrary.SharedObjects.Room;

public class RoomTransferObject
{
    public string RoomId { get; set; } = string.Empty;
    public string HostUserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Participants { get; set; }
    public List<string>? ParticipantIds;
}
