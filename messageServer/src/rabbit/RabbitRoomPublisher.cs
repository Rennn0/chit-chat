using System.Text;
using LLibrary.Guards;
using LLibrary.Logging;
using llibrary.rabbit;
using RabbitMQ.Client;

namespace messageServer.src.rabbit;

public class RabbitRoomPublisher : RabbitBasicObject
{
    private const string _exchange = "rooms";

    public RabbitRoomPublisher(string host, string username, string password)
        : base(host, username, password, nameof(RabbitRoomPublisher)) { }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        Guard.AgainstNull(Connection);
        _channel = await Connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(
            exchange: _exchange,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false
        );

        Diagnostics.LOG_INFO(nameof(RabbitRoomPublisher));
    }

    public async void Publish(string message)
    {
        try
        {
            Guard.AgainstNull(_channel);

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(
                exchange: _exchange,
                routingKey: string.Empty,
                body: messageBytes
            );
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
        }
    }
}
