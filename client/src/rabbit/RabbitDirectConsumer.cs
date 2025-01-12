using LLibrary.Guards;
using LLibrary.Logging;
using llibrary.rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace client.src.rabbit;

public class RabbitDirectConsumer : RabbitBasicObject
{
    private const string _replyQ = "amq.rabbitmq.reply-to";
    private AsyncEventingBasicConsumer? _consumer;
    private readonly Dictionary<string, AsyncEventHandler<BasicDeliverEventArgs>> _callbacks;

    public RabbitDirectConsumer(
        string host,
        string username,
        string password,
        string providedName = nameof(RabbitDirectConsumer),
        int port = 5672
    )
        : base(host, username, password, providedName, port)
    {
        _callbacks = [];
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        Guard.AgainstNull(Connection);
        _channel = await Connection.CreateChannelAsync();

        _consumer = new AsyncEventingBasicConsumer(_channel);

        _consumer.ReceivedAsync += (s, e) =>
        {
            if (
                string.IsNullOrWhiteSpace(e.BasicProperties.CorrelationId)
                || !_callbacks.TryGetValue(
                    e.BasicProperties.CorrelationId,
                    out AsyncEventHandler<BasicDeliverEventArgs>? cb
                )
            )
                return Task.CompletedTask;

            cb.Invoke(s, e);
            _callbacks.Remove(e.BasicProperties.CorrelationId);

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queue: _replyQ, consumer: _consumer, autoAck: true);
    }

    public async Task DirectMessageAsync(
        ReadOnlyMemory<byte> body,
        AsyncEventHandler<BasicDeliverEventArgs> callback,
        string routing = "direct-q"
    )
    {
        try
        {
            Guard.AgainstNull(_channel);

            string correlation = Guid.NewGuid().ToString();
            _callbacks.Add(correlation, callback);

            BasicProperties properties = new() { ReplyTo = _replyQ, CorrelationId = correlation };

            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: routing,
                basicProperties: properties,
                mandatory: true,
                body: body
            );
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
        }
    }
}
