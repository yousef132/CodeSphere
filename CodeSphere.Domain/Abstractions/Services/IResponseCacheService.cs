namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive);

        Task<IEnumerable<T>> GetCachedResponseAsync<T>(string key) where T : class;
    }
}
