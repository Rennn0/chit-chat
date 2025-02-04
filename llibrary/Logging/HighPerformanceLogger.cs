using Microsoft.Extensions.Logging;

namespace llibrary.Logging;

public static class LogEvents
{
    public static readonly EventId CriticalError = new EventId(10, nameof(CriticalError));
    public static readonly EventId Warning = new EventId(15, nameof(Warning));
    public static readonly EventId Information = new EventId(49, nameof(Information));
}

public static class LogginExtensions
{
    private static readonly Action<ILogger, string, string?, Exception?> _critical =
        LoggerMessage.Define<string, string?>(
            LogLevel.Critical,
            LogEvents.CriticalError,
            "Error occured: {message}," + " Hint: {hint}"
        );

    private static readonly Action<ILogger, string, Exception?> _warning =
        LoggerMessage.Define<string>(LogLevel.Warning, LogEvents.Warning, "{message}");

    private static readonly Action<ILogger, string, Exception?> _information =
        LoggerMessage.Define<string>(LogLevel.Information, LogEvents.Information, "{info}");

    public static void LogCritical(
        this ILogger logger,
        string message,
        string hint,
        Exception? exception = null
    ) => _critical(logger, message, hint, exception);

    public static void LogWarning(
        this ILogger logger,
        string message,
        Exception? exception = null
    ) => _warning(logger, message, exception);

    public static void LogInfo(this ILogger logger, string message) =>
        _information(logger, message, null);
}
