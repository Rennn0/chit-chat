using RabbitMQ.Client;

namespace llibrary.rabbit;

public abstract class RabbitBasicObject
{
    protected readonly ConnectionFactory _connectionFactory;
    protected IConnection? _connection;
    protected IChannel? _channel;

    protected RabbitBasicObject(
        string host,
        string username,
        string password,
        string providedName = nameof(RabbitBasicObject),
        int port = 5672
    )
    {
        _connectionFactory = new ConnectionFactory()
        {
            UserName = username,
            Password = password,
            HostName = host,
            Port = port,
            ClientProvidedName = providedName,
        };
    }

    public virtual bool IsInitialized => _connection is not null || _channel is not null;

    public virtual async Task InitializeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }
}
