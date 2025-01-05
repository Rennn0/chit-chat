using System.Collections.Concurrent;
using System.Text;
using LLibrary.Guards;
using RabbitMQ.Client;

namespace messageServer.src.rabbit;

public class RabbitRoomPublisher : BasicPublisher, IDisposable
{
    private const string _exchange = "chit-chat";
    private const string _queue = "rooms";
    private readonly object _lock = new object();
    public static BlockingCollection<string> Messages = [];

    public RabbitRoomPublisher(string host, string username, string password)
        : base(host, username, password)
    {
        Console.WriteLine("[RoomPublisher] On");
    }

    public override async Task CreateQueueTask()
    {
        if (_channel is null || _connection is null)
        {
            await InitializeAsync();
        }

        Guard.AgainstNull(_channel);
        await _channel.ExchangeDeclareAsync(exchange: _exchange, type: ExchangeType.Fanout);
        await _channel.QueueDeclareAsync(
            queue: _queue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await _channel.QueueBindAsync(queue: _queue, exchange: _exchange, routingKey: string.Empty);
        BeginPublishingMessages();
    }

    private void BeginPublishingMessages()
    {
        foreach (string message in Messages.GetConsumingEnumerable())
        {
            PublishMessageTask(message).ConfigureAwait(false);
        }
    }

    protected override async Task PublishMessageTask(string message)
    {
        byte[] body = Encoding.UTF8.GetBytes(message);
        await _channel!.BasicPublishAsync(
            exchange: _exchange,
            routingKey: string.Empty,
            body: body
        );
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _connection?.CloseAsync().Wait();
        _channel?.CloseAsync().Wait();
    }
}
