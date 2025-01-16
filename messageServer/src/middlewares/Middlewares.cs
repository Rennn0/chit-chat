using llibrary.Logging;

namespace messageServer.middlewares;

public class Middlewares
{
    public static async Task Logger(HttpContext context, Func<Task> next)
    {
        string method = context.Request.Method;
        PathString path = context.Request.Path;
        QueryString query = context.Request.QueryString;
        DateTime timestamp = DateTime.UtcNow;

        Diagnostics.LOG_INFO($"Incoming Request: {method} {path}{query}");
        await next.Invoke();
    }
}