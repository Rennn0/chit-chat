using Grpc.Net.Client;
using llibrary.Guards;

namespace client.globals;

public static class Globals
{
    public static string Key { get; set; } = string.Empty;

    public static GrpcChannel GetGrpcChannel(bool insecure = true)
    {
        HttpClientHandler httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        return insecure
            ? GrpcChannel.ForAddress(
                LocalSettings.Default["MessageServerUrl"],
                new GrpcChannelOptions { HttpHandler = httpHandler }
            )
            : GrpcChannel.ForAddress(LocalSettings.Default["MessageServerUrl"]);
    }
}