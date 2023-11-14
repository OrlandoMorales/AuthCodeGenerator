using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArivalBank._2fa.Application.Extensions
{
    public static class RedisCacheExtensions
    {
        public static Task SetAsync<TEntity>(this IDistributedCache cache, string key, TEntity value)
        {
            return SetAsync(cache, key, value, new DistributedCacheEntryOptions());
        }

        public static Task SetAsync<TEntity>(this IDistributedCache cache, string key, TEntity value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions()));

            return cache.SetAsync(key, bytes, options);
        }

        public static Task<TEntity?> GetAsync<TEntity>(this IDistributedCache cache, string key, out TEntity? value)
        {
            var cachedCode = cache.Get(key);
            value = default;

            if (cachedCode == null)
            {
                return Task.FromResult(value);
            }
            else 
            {
                value = JsonSerializer.Deserialize<TEntity>(cachedCode, SerializerOptions());
            }

            return Task.FromResult(value);
        }

        public static bool TryGetValue<TEntity>(this IDistributedCache cache, string key, out TEntity? value)
        {
            var val = cache.Get(key);
            value = default;

            if (val == null) return false;

            value = JsonSerializer.Deserialize<TEntity>(val, SerializerOptions());

            return true;
        }

        private static JsonSerializerOptions SerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
