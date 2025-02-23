using Microsoft.Extensions.Caching.Memory;

public static class CacheExtension
{
    public static void Set2FaToken(this IMemoryCache cache, string token, string userId)
        => cache.Set($"2fa-{userId}", token, TimeSpan.FromMinutes(5));


    public static bool TryGet2FaToken(this IMemoryCache cache, string userId, out string? token)
        => cache.TryGetValue($"2fa-{userId}", out token);

    public static void Remove2FaToken(this IMemoryCache cache, string userId)
        => cache.Remove($"2fa-{userId}");

}