using client.src.rabbit;
using LLibrary.Guards;

namespace client.src.globals;

public class RabbitConsumersFactory
{
    private static RabbitDirectConsumer? _rabbitDirectConsumer;
    private static RabbitRoomConsumer? _rabbitRoomConsumer;

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

    public static async Task<RabbitRoomConsumer> GetRabbitRoomConsumerAsync()
    {
        _rabbitRoomConsumer ??= new RabbitRoomConsumer(
            host: LocalSettings.Default["RabbitHost"],
            username: LocalSettings.Default["RabbitUsername"],
            password: LocalSettings.Default["RabbitPassword"],
            exchange: LocalSettings.Default["RabbitRoomExchange"],
            providedName: nameof(RabbitDirectConsumer),
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_rabbitRoomConsumer.IsInitialized)
        {
            await _rabbitRoomConsumer.InitializeAsync();
        }

        return _rabbitRoomConsumer;
    }
}
