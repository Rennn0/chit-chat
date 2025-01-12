using llibrary.rabbit;

namespace client.src.rabbit;

public class RabbitRoomConsumer : RabbitBasicConsumer
{
    public RabbitRoomConsumer(
        string host,
        string username,
        string password,
        string exchange,
        string providedName = nameof(RabbitRoomConsumer),
        int port = 5672
    )
        : base(host, username, password, exchange, providedName, port) { }
}
