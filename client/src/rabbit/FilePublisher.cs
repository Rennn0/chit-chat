using llibrary.rabbit;

namespace client.rabbit;

public class FilePublisher : RabbitBasicDirectPublisher
{
    public FilePublisher(
        string routingKey,
        string host,
        string username,
        string password,
        string exchange = "amq.direct",
        int port = 5672
    )
        : base(routingKey, host, username, password, exchange, port) { }
}
