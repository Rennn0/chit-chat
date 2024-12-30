using database.entities;
using gRpcProtos;
using RoomTransferObject = llibrary.SharedObjects.Room.RoomTransferObject;

namespace messageServer.extensions;

public static class CreateRoomRequestMapsRoomExtension
{
    public static Room Map(this CreateRoomRequest request)
    {
        return new Room(request.Name, request.HostUserId, request.Description);
    }

    public static Room Map(this RoomTransferObject request)
    {
        throw new NotImplementedException();
    }

    public static RoomTransferObject Map(this Room room)
    {
        return new RoomTransferObject()
        {
            Description = room.Description,
            HostUserId = room.HostUserId,
            Name = room.Name,
            RoomId = room.Id,
            Participants = room.Users.Count,
            ParticipantIds = room.Users,
        };
    }
}
