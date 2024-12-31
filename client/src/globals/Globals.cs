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
                Properties.Resources.MessageServerUrl,
                new GrpcChannelOptions { HttpHandler = httpHandler }
            )
            : GrpcChannel.ForAddress(Properties.Resources.MessageServerUrl);
    }
}
