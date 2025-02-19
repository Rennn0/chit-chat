using llibrary.Logging;
using llibrary.Rabbit;
using Microsoft.Extensions.Logging;

namespace fileServerCs;

/// <summary>
///     bazashi inaxeba files metadata
///     realuri file meore serverzea da iqidan wamoiebs
/// </summary>
public class RabbitFileConsumer : RabbitBasicDirectConsumer
{
    private readonly ILogger<IInformationLogger> _infoLogger;

    public RabbitFileConsumer(
        string queue,
        string routingKey,
        string host,
        string username,
        string password,
        ILogger<IInformationLogger> infoLogger,
        int port = 5672
    )
        : base(queue, routingKey, host, username, password, port)
    {
        _infoLogger = infoLogger;
    }

    public RabbitFileConsumer(
        string queue,
        string routingKey,
        RabbitSettings settings,
        ILogger<IInformationLogger> infoLogger
    )
        : base(routingKey, settings.Host, settings.Username, settings.Password, queue)
    {
        _infoLogger = infoLogger;
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _infoLogger.LogInfo("File consumer started");
    }
}