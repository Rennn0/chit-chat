using llibrary.rabbit;

namespace client.src.rabbit;

public class RoomConsumer : RabbitBasicFanoutConsumer
{
    public RoomConsumer(
        string host,
        string username,
        string password,
        string exchange,
        int port = 5672
    )
        : base(host, username, password, port) { }
}
