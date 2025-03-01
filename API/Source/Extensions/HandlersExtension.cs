using System.Reflection;
using API.Source.Handlers;

namespace API.Source.Extensions;

public static class HandlersExtension
{
    public static void RegisterHandlersFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        IEnumerable<Type> handlerTypes = assembly
            .GetTypes()
            .Where(t =>
                t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                    )
            );

        foreach (Type handlerType in handlerTypes)
        {
            IEnumerable<Type> interfaces = handlerType
                .GetInterfaces()
                .Where(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                );

            foreach (Type @interface in interfaces)
            {
                services.AddScoped(@interface, handlerType);
            }
        }

        services.AddScoped(typeof(IRequestPipeline<,>), typeof(RequestPipeline<,>));
    }
}
