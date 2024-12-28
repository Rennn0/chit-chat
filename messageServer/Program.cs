using database;
using database.interfaces;
using database.mongo;
using messageServer;
using messageServer.protoServices;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings))
);

builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    MongoDbSettings settingss = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoDbContext(settingss.ConnectionString, settingss.Database);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    MongoDbContext mongo = sp.GetRequiredService<MongoDbContext>();
    return mongo.Database;
});

builder.Services.AddScoped(typeof(IDatabaseAdapter<>), typeof(MongoDbAdapter<>));

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.MapGrpcService<MessageService>();
app.Run();
