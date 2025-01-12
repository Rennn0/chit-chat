using LLibrary.Guards;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace llibrary.rabbit;

public abstract class RabbitBasicConsumer : RabbitBasicObject
{
    private readonly string _exchange;
    protected AsyncEventingBasicConsumer? _consumer;

    protected RabbitBasicConsumer(
        string host,
        string username,
        string password,
        string exchange,
        string providedName = "RabbitBasicObject",
        int port = 5672
    )
        : base(host, username, password, providedName, port)
    {
        _exchange = exchange;
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
        _consumer = new AsyncEventingBasicConsumer(_channel);

        QueueDeclareOk q = await _channel.QueueDeclareAsync(
            exclusive: true,
            durable: false,
            autoDelete: true
        );
        await _channel.QueueBindAsync(
            queue: q.QueueName,
            exchange: _exchange,
            routingKey: string.Empty
        );
        await _channel.BasicConsumeAsync(queue: q.QueueName, autoAck: true, consumer: _consumer);
    }

    public virtual void AttachCallback(AsyncEventHandler<BasicDeliverEventArgs> callback)
    {
        Guard.AgainstNull(_channel);
        Guard.AgainstNull(_consumer);
        _consumer.ReceivedAsync += callback;
    }
}
