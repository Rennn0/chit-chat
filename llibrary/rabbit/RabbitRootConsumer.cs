using llibrary.Guards;
using RabbitMQ.Client.Events;

namespace llibrary.rabbit;

/// <summary>
///     nebismieri consumer aimplementirebs am klass
///     tito instancze titp consumer object iqneba
///     romelzec ramdenime callback daemateba
///     shvil klasshi InitializeAsync() gamosadzaxebelia sasurveli Q shesaqmnelad
/// </summary>
public abstract class RabbitRootConsumer : RabbitRootObject
{
    protected readonly string _exchange;
    protected AsyncEventingBasicConsumer? _consumer;

    protected RabbitRootConsumer(
        string exchange,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port)
    {
        _exchange = exchange;
    }

    // TODO rame propertebi sheileba daematos
    public virtual void AttachCallback(AsyncEventHandler<BasicDeliverEventArgs> callback)
    {
        Guard.AgainstNull(_channel);
        Guard.AgainstNull(_consumer);

        _consumer.ReceivedAsync += callback;
    }
}