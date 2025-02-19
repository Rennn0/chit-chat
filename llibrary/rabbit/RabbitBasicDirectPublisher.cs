using llibrary.Guards;

namespace llibrary.Rabbit;

public class RabbitBasicDirectPublisher : RabbitRootPublisher
{
    public RabbitBasicDirectPublisher(
        string routingKey,
        string host,
        string username,
        string password,
        string exchange = "amq.direct",
        int port = 5672
    )
        : base(exchange, routingKey, host, username, password, port)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
    }
}