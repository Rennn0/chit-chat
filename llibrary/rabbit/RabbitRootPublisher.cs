using System.Text;
using llibrary.Guards;
using llibrary.Logging;
using RabbitMQ.Client;

namespace llibrary.Rabbit;

/// <summary>
///     override InitializeAsync() shvil klasshi
/// </summary>
public abstract class RabbitRootPublisher : RabbitRootObject
{
    protected readonly string _exchange;
    protected readonly string _routingKey;

    protected RabbitRootPublisher(
        string exchange,
        string routingKey,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port)
    {
        _exchange = exchange;
        _routingKey = routingKey;
    }

    // TODO basic properties dasamatebelia parametrebshi
    public virtual async void Publish(string message)
    {
        try
        {
            Guard.AgainstNull(_channel);

            byte[] msgBytes = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(
                exchange: _exchange,
                routingKey: _routingKey,
                mandatory: true,
                body: msgBytes
            );
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
        }
    }
}