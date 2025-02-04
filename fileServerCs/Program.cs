using fileServerCs;
using llibrary.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

Dependencies
    .Services.AddTransient<ILogger<ICriticalLogger>>(sp =>
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                "critical_logs.txt",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal
            )
            .CreateLogger();
        using ILoggerFactory criticalFactory = LoggerFactory.Create(builder =>
            builder.AddSerilog().AddDebug().AddConsole()
        );
        return criticalFactory.CreateLogger<ICriticalLogger>();
    })
    .AddTransient<ILogger<IWarningLogger>>(sp =>
    {
        using ILoggerFactory warningFactory = LoggerFactory.Create(builder =>
            builder.AddDebug().AddConsole()
        );
        return warningFactory.CreateLogger<IWarningLogger>();
    })
    .AddTransient<ILogger<IInformationLogger>>(sp =>
    {
        using ILoggerFactory infoFactory = LoggerFactory.Create(builder => builder.AddDebug());
        return infoFactory.CreateLogger<IInformationLogger>();
    })
    .AddSingleton<RabbitFileConsumer>(sp => new RabbitFileConsumer(
        queue: "files",
        routingKey: "files",
        Settings.RabbitSettings,
        infoLogger: sp.GetRequiredService<ILogger<IInformationLogger>>()
    ));

Dependencies.Provider = Dependencies.Services.BuildServiceProvider();

RabbitFileConsumer fileConsumer = Dependencies.Provider.GetRequiredService<RabbitFileConsumer>();
await fileConsumer.InitializeAsync();
fileConsumer.AttachCallback(Callbacks.SaveFileAsync);

Console.ReadKey();
