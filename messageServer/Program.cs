using database;
using database.interfaces;
using database.mongo;
using LLibrary.Guards;
using messageServer;
using messageServer.handlers;
using messageServer.middlewares;
using messageServer.protoServices;
using messageServer.src.protoServices;
using messageServer.src.rabbit;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(so =>
{
    so.ConfigureEndpointDefaults(a => a.Protocols = HttpProtocols.Http2);
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

RabbitSettings config = Guard.AgainstNull(
    builder.Configuration.GetSection("RabbitSettings").Get<RabbitSettings>()
);
builder.Services.AddSingleton<RoomPublisher>(
    new RoomPublisher(config.Host, config.Username, config.Password)
);
builder.Services.AddSingleton<DirectPublisher>(
    new DirectPublisher(config.Host, config.Username, config.Password)
);

builder.Services.AddScoped(typeof(IDatabaseAdapter<>), typeof(MongoDbAdapter<>));

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.Use(Middlewares.Logger);

app.MapGrpcService<MessageExchange>();
app.MapGrpcService<RoomExchange>();

app.MapGet(pattern: "/sync", requestDelegate: Handlers.SyncHandler());

using (IServiceScope scope = app.Services.CreateScope())
{
    RoomPublisher roomPublisher = scope.ServiceProvider.GetRequiredService<RoomPublisher>();
    DirectPublisher directPublisher = scope.ServiceProvider.GetRequiredService<DirectPublisher>();

    await roomPublisher.InitializeAsync();
    await directPublisher.InitializeAsync();
    // TODO erti connection bevri channel
}

app.Run();


// TODO rabbitshi monacemebis gacvlis proto rame
