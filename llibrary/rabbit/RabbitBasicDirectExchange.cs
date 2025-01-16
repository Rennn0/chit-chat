using System.Text;
using llibrary.Guards;
using llibrary.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace llibrary.rabbit;

/// <summary>
///     es exchange direct-reply-to iyenebs temp Q sheqmnis magivrad
/// </summary>
public abstract class DirectPublisher : RabbitRootObject
{
    private readonly string _queue;

    protected DirectPublisher(
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port)
    {
        _queue = queue;
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);
        _channel = await Connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: _queue, durable: true, autoDelete: false);
        AsyncEventingBasicConsumer consumer = new(_channel);
        consumer.ReceivedAsync += Consumer_ReceivedAsync;

        await _channel.BasicConsumeAsync(queue: _queue, autoAck: true, consumer: consumer);
    }

    private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            Guard.AgainstNull(_channel);
            Guard.AgainstNull(@event.BasicProperties.ReplyTo);

            string message = Encoding.UTF8.GetString(@event.Body.ToArray());

            BasicProperties replyProps =
                new() { CorrelationId = @event.BasicProperties.CorrelationId };

            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: @event.BasicProperties.ReplyTo,
                mandatory: true,
                basicProperties: replyProps,
                body: ProcessMsg(message)
            );
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
        }
    }

    public abstract byte[] ProcessMsg(string msg);
}

/// <summary>
///     es exchange direct-reply-to iyenebs temp Q sheqmnis magivrad
/// </summary>
public abstract class DirectConsumer : RabbitRootObject
{
    private readonly string _queue;
    private const string _replyQ = "amq.rabbitmq.reply-to";
    private AsyncEventingBasicConsumer? _consumer;
    private readonly Dictionary<string, AsyncEventHandler<BasicDeliverEventArgs>> _callbacks;

    protected DirectConsumer(
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port)
    {
        _queue = queue;
        _callbacks = [];
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
        _consumer = new AsyncEventingBasicConsumer(_channel);

        _consumer.ReceivedAsync += async (s, e) =>
        {
            if (
                string.IsNullOrWhiteSpace(e.BasicProperties.CorrelationId)
                || !_callbacks.TryGetValue(
                    e.BasicProperties.CorrelationId,
                    out AsyncEventHandler<BasicDeliverEventArgs>? cb
                )
            )
                return;

            await cb.Invoke(s, e);
            _callbacks.Remove(e.BasicProperties.CorrelationId);

            return;
        };

        await _channel.BasicConsumeAsync(queue: _replyQ, consumer: _consumer, autoAck: true);
    }

    public async Task DirectMessageAsync(
        ReadOnlyMemory<byte> body,
        AsyncEventHandler<BasicDeliverEventArgs> callback
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
                routingKey: _queue,
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