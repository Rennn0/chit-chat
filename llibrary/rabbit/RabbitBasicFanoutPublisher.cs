using System.Text;
using LLibrary.Guards;
using RabbitMQ.Client;

namespace llibrary.rabbit;

public abstract class RabbitBasicFanoutPublisher : RabbitRootObject
{
    protected RabbitBasicFanoutPublisher(
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port) { }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
    }

    public virtual async void Publish(string message)
    {
        Guard.AgainstNull(_channel);
        byte[] msgBytes = Encoding.UTF8.GetBytes(message);
        await _channel.BasicPublishAsync(
            exchange: "amq.fanout",
            routingKey: string.Empty,
            mandatory: true,
            body: msgBytes
        );
    }
}
