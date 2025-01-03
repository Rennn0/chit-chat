using generator;
using Grpc.Net.Client;

namespace client.globals;

public static class Globals
{
    public static GrpcChannel GetGrpcChannel(bool insecure = true)
    {
        HttpClientHandler httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        return insecure
            ? GrpcChannel.ForAddress(
                RuntimeTrexSettings.Get(TrexSettings.MessageServerUrl),
                new GrpcChannelOptions { HttpHandler = httpHandler }
            )
            : GrpcChannel.ForAddress(RuntimeTrexSettings.Get(TrexSettings.MessageServerUrl));
    }
}
