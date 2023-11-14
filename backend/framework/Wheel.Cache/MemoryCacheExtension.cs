using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Memory
{
    public static class MemoryCacheExtension
    {
        public static void SetAbsoluteExpirationRelativeToNow<T>(this IMemoryCache cache, string key, T value, TimeSpan absoluteExpiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(absoluteExpiration);
            cache.Set(key, value, cacheEntryOptions);
        }
        public static void SetAbsoluteExpiration<T>(this IMemoryCache cache, string key, T value, DateTimeOffset absoluteExpiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(absoluteExpiration);
            cache.Set(key, value, cacheEntryOptions);
        }
        public static void SetSlidingExpiration<T>(this IMemoryCache cache, string key, T value, TimeSpan slidingExpiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(slidingExpiration);

            cache.Set(key, value, cacheEntryOptions);
        }
    }
}
