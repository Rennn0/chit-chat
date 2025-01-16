using llibrary.rabbit;

namespace client.rabbit;

public class RoomConsumer : RabbitBasicFanoutConsumer
{
    public RoomConsumer(string host, string username, string password, int port = 5672)
        : base(host, username, password)
    {
    }
}