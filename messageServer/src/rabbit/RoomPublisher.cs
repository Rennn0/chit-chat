using llibrary.rabbit;

namespace messageServer.rabbit;

public class RoomPublisher : RabbitBasicFanoutPublisher
{
    public RoomPublisher(string host, string username, string password)
        : base(host: host, username: username, password: password)
    {
    }
}