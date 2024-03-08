using Microsoft.Extensions.Caching.Distributed;

namespace AviasalesApi.Options
{
    public static class CacheOptions
    {
        public static DistributedCacheEntryOptions DefaultExpiration =>
            new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) };
    }
}
