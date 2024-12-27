using messageServer;
using messageServer.src.protoServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings))
);
builder.Services.AddGrpc();

WebApplication app = builder.Build();
app.MapGrpcService<MessageService>();
app.Run();
