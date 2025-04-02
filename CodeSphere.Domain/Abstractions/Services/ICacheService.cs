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
        void CacheContestStanding(ContestPoints points, UserToCache user, int ContestId);
        IReadOnlyList<StandingDto> GetContestStanding(int contestId, int start, int stop);
        void CacheUserSubmission(SubmissionToCache submission, string userId, int contestId);
        bool IsUserSolvedTheProblem(string userId, int contestId, int problemId);
    }
}
