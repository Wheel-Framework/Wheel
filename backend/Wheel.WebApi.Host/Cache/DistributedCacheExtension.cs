using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheExtension
    {
        static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
        {
            var value = await cache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrWhiteSpace(value))
                return default(T);
            return JsonSerializer.Deserialize<T>(value, JsonSerializerOptions);
        }
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default)
        {
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonSerializerOptions), cancellationToken);
        }
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions distributedCacheEntryOptions, CancellationToken cancellationToken = default)
        {
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonSerializerOptions), distributedCacheEntryOptions, cancellationToken);
        }
        public static async Task SetAbsoluteExpirationRelativeToNowAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeSpan
            };
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonSerializerOptions), options, cancellationToken);
        }
        public static async Task SetAbsoluteExpirationAsync<T>(this IDistributedCache cache, string key, T value, DateTimeOffset dateTimeOffset, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = dateTimeOffset
            };
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonSerializerOptions), options, cancellationToken);
        }
        public static async Task SetSlidingExpirationAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan slidingExpiration, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonSerializerOptions), options, cancellationToken);
        }
    }
}
