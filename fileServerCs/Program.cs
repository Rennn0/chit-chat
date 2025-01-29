using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Microsoft.Extensions.Logging;

//const string fileConsumerQRk = "files";

//RabbitFileConsumer fileConsumer = new RabbitFileConsumer(
//    queue: fileConsumerQRk,
//    routingKey: fileConsumerQRk,
//    settings: Settings.RabbitSettings
//);
//await fileConsumer.InitializeAsync();
//fileConsumer.AttachCallback(Callbacks.SaveFileAsync);

using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole().AddDebug().SetMinimumLevel(LogLevel.Information)
);

ILogger logger = loggerFactory.CreateLogger<Program>();

MemcachedClientConfiguration config = new MemcachedClientConfiguration(
    loggerFactory,
    new MemcachedClientOptions()
);
config.AddServer("localhost", 11211);
config.Protocol = MemcachedProtocol.Binary;

MemcachedClient client = new MemcachedClient(loggerFactory, config);
Console.WriteLine("Adding kv");

await client.SetAsync("luka", "danelia", TimeSpan.FromSeconds(5));
int counter = 0;
string? value;
do
{
    value = client.Get<string>("luka");
    if (value != null)
    {
        counter++;
        logger.LogInformation($"Retrieved: 'luka' -> '{value}' {counter}");
    }
    else
    {
        logger.LogWarning("Key 'luka' not found in Memcached.");
    }
} while (!string.IsNullOrEmpty(value));

Console.ReadKey();
