using database;
using database.interfaces;
using database.mongo;
using LLibrary.Logging;
using messageServer;
using messageServer.handlers;
using messageServer.middlewares;
using messageServer.protoServices;
using messageServer.rabbit;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

Diagnostics.LOG_ERROR("kakashka");
Diagnostics.LOG_INFO("kakashka");
Diagnostics.LOG_WARNING("kakashka");
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(so =>
{
    so.ConfigureEndpointDefaults(a => a.Protocols = HttpProtocols.Http2);
    so.ListenAnyIP(5000);
});

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings))
);
builder.Services.Configure<RabbitSettings>(
    builder.Configuration.GetSection(nameof(RabbitSettings))
);

builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    MongoDbSettings settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoDbContext(settings.ConnectionString, settings.Database);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    MongoDbContext mongo = sp.GetRequiredService<MongoDbContext>();
    return mongo.Database;
});

builder.Services.AddScoped(typeof(IDatabaseAdapter<>), typeof(MongoDbAdapter<>));

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.Use(Middlewares.Logger);

app.MapGrpcService<MessageExchange>();
app.MapGrpcService<RoomExchange>();

RabbitSettings rabbitSettings = app.Services.GetRequiredService<IOptions<RabbitSettings>>().Value;
new Thread(() =>
{
    RabbitRoomPublisher publisher =
        new(rabbitSettings.Host, rabbitSettings.Username, rabbitSettings.Password);
    publisher.CreateQueueTask().ConfigureAwait(false);
})
{
    IsBackground = true,
}.Start();

app.MapGet(pattern: "/sync", requestDelegate: Handlers.SyncHandler());
app.Run();


// TODO rabbitshi monacemebis gacvlis proto rame
