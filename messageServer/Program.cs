using messageServer.src.protoServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.MapGrpcService<MessageService>();

app.Run();
