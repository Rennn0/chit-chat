using RabbitMQ.Client;

namespace llibrary.Rabbit;

public abstract class RabbitRootObject
{
    protected readonly ConnectionFactory _connectionFactory;
    protected IChannel? _channel;
    protected static IConnection? Connection { get; set; }

    protected RabbitRootObject(string host, string username, string password, int port = 5672)
    {
        _connectionFactory = new ConnectionFactory()
        {
            UserName = username,
            Password = password,
            HostName = host,
            Port = port,
            AutomaticRecoveryEnabled = true,
        };
    }

    public virtual bool CompletedInitialization => Connection is not null && _channel is not null;

    public virtual async Task InitializeAsync()
    {
        Connection ??= await _connectionFactory.CreateConnectionAsync();
    }
}

// TODO exchanges gadawyoba