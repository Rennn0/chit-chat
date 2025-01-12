using LLibrary.Logging;
using llibrary.rabbit;

namespace messageServer.src.rabbit;

public class RoomPublisher : RabbitBasicFanoutPublisher
{
    public RoomPublisher(string host, string username, string password)
        : base(host: host, username: username, password: password) { }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        Diagnostics.LOG_INFO(nameof(RoomPublisher));
    }
}
