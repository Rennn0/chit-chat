using llibrary.rabbit;

namespace messageServer.src.rabbit;

public class RoomPublisher : RabbitBasicFanoutPublisher
{
    public RoomPublisher(string host, string username, string password)
        : base(host: host, username: username, password: password) { }
}
