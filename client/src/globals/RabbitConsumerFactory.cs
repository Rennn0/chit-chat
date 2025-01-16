using client.rabbit;
using llibrary.Guards;

namespace client.globals;

public class RabbitConsumerFactory
{
    private static SettingsConsumer? _rabbitDirectConsumer;
    private static RoomConsumer? _rabbitRoomConsumer;
    private static FilePublisher? _filePublisher;

    public static async Task<SettingsConsumer> GetDirectConsumerAsync()
    {
        _rabbitDirectConsumer ??= new SettingsConsumer(
            queue: "rpc.settings",
            host: LocalSettings.Default["RabbitHost"],
            username: LocalSettings.Default["RabbitUsername"],
            password: LocalSettings.Default["RabbitPassword"],
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_rabbitDirectConsumer.CompletedInitialization)
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
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_rabbitRoomConsumer.CompletedInitialization)
        {
            await _rabbitRoomConsumer.InitializeAsync();
        }

        return _rabbitRoomConsumer;
    }

    public static async Task<FilePublisher> GetFilePublisherAsync()
    {
        _filePublisher = new FilePublisher(
            routingKey: "files",
            host: LocalSettings.Default["RabbitHost"],
            username: LocalSettings.Default["RabbitUsername"],
            password: LocalSettings.Default["RabbitPassword"],
            port: int.Parse(LocalSettings.Default["RabbitPort"])
        );

        if (!_filePublisher.CompletedInitialization)
        {
            await _filePublisher.InitializeAsync();
        }

        return _filePublisher;
    }
}
