using llibrary.Logging;

namespace llibrary.Network;

public class HttpHandlers
{
    private static readonly HttpClientHandler _clientHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, err) => true,
    };

    public static async Task<string> Sync()
    {
        using HttpClient client = new(_clientHandler);
        client.DefaultRequestVersion = new Version(2, 0);
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;

        const string url = "http://localhost:5000/sync";
        const int maxRetries = 23;
        const int delayMs = 5000;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    continue;

                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                Diagnostics.LOG_ERROR(e.Message);
                await Task.Delay(delayMs);
            }
        }

        return string.Empty;
    }
}