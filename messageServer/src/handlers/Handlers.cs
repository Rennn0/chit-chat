namespace messageServer.handlers;

public class Handlers
{
    public static RequestDelegate SyncHandler()
    {
        //  TODO prosta keys abrunebs axla mere meti security unda
        return async context =>
        {
            await context.Response.WriteAsync("JqWy*ezwx$8#n2a0aYAka@haYP2cWbk^");
        };
    }
}
