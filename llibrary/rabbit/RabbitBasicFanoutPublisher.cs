using llibrary.Guards;

namespace llibrary.Rabbit;

public abstract class RabbitBasicFanoutPublisher : RabbitRootPublisher
{
    protected RabbitBasicFanoutPublisher(
        string host,
        string username,
        string password,
        string exchange = "amq.fanout",
        int port = 5672
    )
        : base(exchange, routingKey: string.Empty, host, username, password, port)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
    }
}