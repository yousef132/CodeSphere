using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive);
        Task UpdateContestCache(Submit submission);
        Task<string> GetCachedResponseAsync(string key);
    }
}
