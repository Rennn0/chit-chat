using llibrary.Logging;
using llibrary.rabbit;

namespace fileServerCs;

/// <summary>
///     bazashi inaxeba files metadata
///     realuri file meore serverzea da iqidan wamoiebs
/// </summary>
public class RabbitFileConsumer : RabbitBasicDirectConsumer
{
    public RabbitFileConsumer(
        string queue,
        string routingKey,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(queue, routingKey, host, username, password, port)
    {
    }

    public RabbitFileConsumer(string queue, string routingKey, RabbitSettings settings)
        : base(routingKey, settings.Host, settings.Username, settings.Password, queue)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Diagnostics.LOG_INFO("File consumer");
    }
}