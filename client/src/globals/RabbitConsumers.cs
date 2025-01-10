using client.rabbit;
using LLibrary.Guards;

namespace client.globals;

public class RabbitConsumersFactory
{
    private static RabbitDirectConsumer? _rabbitDirectConsumer;

    public static async Task<RabbitDirectConsumer> GetRabbitDirectConsumerAsync()
    {
        _rabbitDirectConsumer ??= new RabbitDirectConsumer(
            host: LocalSettings.Default["RabbitHost"],
            username: LocalSettings.Default["RabbitUsername"],
            password: LocalSettings.Default["RabbitPassword"],
            providedName: nameof(RabbitDirectConsumer),
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_rabbitDirectConsumer.IsInitialized)
        {
            await _rabbitDirectConsumer.InitializeAsync();
        }

        return _rabbitDirectConsumer;
    }
}
