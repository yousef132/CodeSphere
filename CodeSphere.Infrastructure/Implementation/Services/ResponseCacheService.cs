using CodeSphere.Domain.Abstractions.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _Database;

        public ResponseCacheService(IConnectionMultiplexer multiplexer)
        {
            _Database = multiplexer.GetDatabase();
        }



        public async Task CacheResponseAsync(string key, object Response, TimeSpan timeToiLive)
        {
            if (Response is null)
                return;

            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(Response, serializeOptions);
            await _Database.StringSetAsync(key, serializedResponse, timeToiLive);
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var value = await _Database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return null;

            return (value);
        }
    }
}
