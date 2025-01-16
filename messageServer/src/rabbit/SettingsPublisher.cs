using System.Text;
using llibrary.rabbit;

namespace messageServer.rabbit;

public class SettingsPublisher : DirectPublisher
{
    public SettingsPublisher(
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(queue, host, username, password, port)
    {
    }

    public override byte[] ProcessMsg(string msg)
    {
        string response = $"{msg} mivige";
        return Encoding.UTF8.GetBytes(response);
    }
}