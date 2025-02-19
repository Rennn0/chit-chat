using Microsoft.Extensions.DependencyInjection;

namespace fileServerCs;

public static class Dependencies
{
    public static IServiceCollection Services { get; } = new ServiceCollection();
    public static IServiceProvider? Provider { get; set; }
}
