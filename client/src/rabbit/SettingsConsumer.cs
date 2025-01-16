using llibrary.rabbit;

namespace client.rabbit;

public class SettingsConsumer : DirectConsumer
{
    public SettingsConsumer(
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(queue, host, username, password, port)
    {
    }
}