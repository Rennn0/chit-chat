using llibrary.Guards;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace llibrary.Rabbit;

public class RabbitBasicDirectConsumer : RabbitRootConsumer
{
    private string? _queue;
    private string _routingKey;

    public RabbitBasicDirectConsumer(
        string routingKey,
        string host,
        string username,
        string password,
        string? queue = null,
        int port = 5672
    )
        : base(exchange: "amq.direct", host, username, password, port)
    {
        _queue = queue;
        _routingKey = routingKey;
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
        _consumer = new AsyncEventingBasicConsumer(_channel);

        if (_queue is null)
        {
            QueueDeclareOk queueDeclareResult = await _channel.QueueDeclareAsync(
                durable: false,
                exclusive: true,
                autoDelete: true,
                arguments: null
            );

            _queue = queueDeclareResult.QueueName;
        }
        else
        {
            await _channel.QueueDeclareAsync(
                queue: _queue,
                durable: false,
                exclusive: false,
                autoDelete: true
            );
        }

        await _channel.QueueBindAsync(queue: _queue, exchange: _exchange, routingKey: _queue);
        await _channel.BasicConsumeAsync(queue: _queue, autoAck: true, consumer: _consumer);
    }
}