using RabbitMQ.Client;

namespace messageServer.rabbit;

public abstract class BasicPublisher
{
    protected readonly ConnectionFactory _connectionFactory;
    protected IConnection? _connection;
    protected IChannel? _channel;

    protected BasicPublisher(string host, string username, string password)
    {
        _connectionFactory = new ConnectionFactory()
        {
            UserName = username,
            Password = password,
            HostName = host,
        };
    }

    protected async Task InitializeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public abstract Task CreateQueueTask();
    protected abstract Task PublishMessageTask(string message);
}
