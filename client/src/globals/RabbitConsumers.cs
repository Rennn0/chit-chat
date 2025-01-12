using client.src.rabbit;
using LLibrary.Guards;

namespace client.src.globals;

public class RabbitConsumersFactory
{
    private static DirectConsumer? _rabbitDirectConsumer;
    private static RoomConsumer? _rabbitRoomConsumer;

    public static async Task<DirectConsumer> GetDirectConsumerAsync()
    {
        _rabbitDirectConsumer ??= new DirectConsumer(
            host: LocalSettings.Default["RabbitHost"],
            username: LocalSettings.Default["RabbitUsername"],
            password: LocalSettings.Default["RabbitPassword"],
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_rabbitDirectConsumer.IsInitialized)
        {
            await _rabbitDirectConsumer.InitializeAsync();
        }

        return _rabbitDirectConsumer;
    }

    public static async Task<RoomConsumer> GetRoomConsumerAsync()
    {
        _rabbitRoomConsumer ??= new RoomConsumer(
            host: LocalSettings.Default["RabbitHost"],
            username: LocalSettings.Default["RabbitUsername"],
            password: LocalSettings.Default["RabbitPassword"],
            exchange: LocalSettings.Default["RabbitRoomExchange"],
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_rabbitRoomConsumer.IsInitialized)
        {
            await _rabbitRoomConsumer.InitializeAsync();
        }

        return _rabbitRoomConsumer;
    }
}
