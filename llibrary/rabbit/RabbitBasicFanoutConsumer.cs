using LLibrary.Guards;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace llibrary.rabbit;

public class RabbitBasicFanoutConsumer : RabbitRootObject
{
    protected AsyncEventingBasicConsumer? _consumer;

    public RabbitBasicFanoutConsumer(string host, string username, string password, int port = 5672)
        : base(host, username, password, port) { }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
        QueueDeclareOk q = await _channel.QueueDeclareAsync(
            durable: false,
            exclusive: true,
            autoDelete: true,
            arguments: null
        );
        await _channel.QueueBindAsync(
            queue: q.QueueName,
            exchange: "amq.fanout",
            routingKey: string.Empty
        );

        _consumer = new AsyncEventingBasicConsumer(_channel);
        await _channel.BasicConsumeAsync(queue: q.QueueName, autoAck: true, consumer: _consumer);
    }

    public virtual void AttachCallback(AsyncEventHandler<BasicDeliverEventArgs> callback)
    {
        Guard.AgainstNull(_channel);
        Guard.AgainstNull(_consumer);

        _consumer.ReceivedAsync += callback;
    }
}
