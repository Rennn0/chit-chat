using Microsoft.Extensions.DependencyInjection;

namespace fileServerCs;

public static class Dependencies
{
    public static IServiceCollection Services = new ServiceCollection();
    public static IServiceProvider? Provider;
}
