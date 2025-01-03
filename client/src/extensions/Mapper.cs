using client.bindings;
using llibrary.SharedObjects.Room;

namespace client.extensions;

public static class Mapper
{
    public static RoomsControlBinding Map(this RoomTransferObject arg) =>
        new RoomsControlBinding()
        {
            G_description = arg.Description,
            G_hostUserId = arg.HostUserId,
            G_participants = arg.Participants,
            G_roomId = arg.RoomId,
            G_roomName = arg.Name,
        };

    public static RoomsControlBinding Map(this gRpcProtos.RoomTransferObject arg) =>
        new RoomsControlBinding()
        {
            G_description = arg.Description,
            G_hostUserId = arg.HostUserId,
            G_participants = arg.Participants,
            G_roomId = arg.RoomId,
            G_roomName = arg.Name,
        };
}
