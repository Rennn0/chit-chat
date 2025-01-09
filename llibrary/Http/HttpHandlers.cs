using LLibrary.Logging;

namespace llibrary.Http;

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

        const string url = "http://20.199.82.7:5000/sync";
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
            throw;
        }

        return string.Empty;
    }
}
