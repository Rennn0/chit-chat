using RabbitMQ.Client;

namespace llibrary.rabbit;

public abstract class RabbitBasicObject
{
    protected readonly ConnectionFactory _connectionFactory;
    protected IChannel? _channel;
    protected static IConnection? Connection { get; set; }

    protected RabbitBasicObject(string host, string username, string password, int port = 5672)
    {
        _connectionFactory = new ConnectionFactory()
        {
            UserName = username,
            Password = password,
            HostName = host,
            Port = port,
        };
    }

    public virtual bool IsInitialized => Connection is not null && _channel is not null;

    public virtual async Task InitializeAsync()
    {
        Connection ??= await _connectionFactory.CreateConnectionAsync();
    }
}

// TODO exchanges gadawyoba
