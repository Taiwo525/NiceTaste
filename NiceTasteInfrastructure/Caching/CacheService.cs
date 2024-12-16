using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    }

    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;
        private readonly ILogger<CacheService> _logger;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public CacheService(
            IDistributedCache cache,
            IOptions<CacheSettings> settings,
            ILogger<CacheService> logger)
        {
            _cache = cache;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var cachedValue = await _cache.GetStringAsync(GetFullKey(key));
                return cachedValue == null ? default : JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cached value for key {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(_settings.DefaultExpirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(_settings.SlidingExpirationMinutes)
                };

                var serializedValue = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(GetFullKey(key), serializedValue, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cached value for key {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(GetFullKey(key));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cached value for key {Key}", key);
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            var value = await GetAsync<T>(key);
            if (value != null)
            {
                return value;
            }

            try
            {
                await _semaphore.WaitAsync();

                // Double check after acquiring lock
                value = await GetAsync<T>(key);
                if (value != null)
                {
                    return value;
                }

                value = await factory();
                await SetAsync(key, value, expiration);
                return value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private string GetFullKey(string key)
        {
            return $"{_settings.KeyPrefix}:{key}";
        }
    }

    public class CacheSettings
    {
        public string KeyPrefix { get; set; } = "identity";
        public int DefaultExpirationMinutes { get; set; } = 30;
        public int SlidingExpirationMinutes { get; set; } = 10;
    }
}
