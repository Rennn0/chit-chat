using database.entities;
using database.interfaces;
using Grpc.Core;
using gRpcProtos;

namespace messageServer.protoServices;

// TODO rabit publisheri esunda gaxdes
public class RoomExchange : RoomExchangeService.RoomExchangeServiceBase
{
    private readonly IDatabaseAdapter<Room> _roomDb;

    public RoomExchange(IDatabaseAdapter<Room> roomDb)
    {
        _roomDb = roomDb;
    }

    public override async Task<ListAvailableRoomsResponse> ListAvailableRooms(
        ListAvailableRoomsRequest request,
        ServerCallContext context
    )
    {
        IEnumerable<Room> rooms = await _roomDb.GetAllAsync();
        ListAvailableRoomsResponse response = new();
        response.Rooms.Add(
            rooms.Select(r => new RoomTransferObject()
            {
                Description = r.Description ?? "",
                HostUserId = r.HostUserId,
                Name = r.Name,
                RoomId = r.Id,
                Participants = r.Users.Count,
            })
        );
        return response;
    }
}
