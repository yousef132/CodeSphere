using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using CodeSphere.Domain.Responses.Contest;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface ICacheService
    {
        Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive);
        Task UpdateContestCache(Submit submission);
        Task<string> GetCachedResponseAsync(string key);
        void UpdateStanding(ContestPoints points, UserToCache user, int ContestId);
        Task<IReadOnlyList<ContestStandingResposne>> GetContestStanding(int contestId, int start, int stop);
        Task TestCache();
    }
}
