using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface ISubmissionRepository
    {
        Task<IQueryable<Submit>> GetAllSubmissions(int problemId, string userId);
        IQueryable<Submit> GetSolvedSubmissions(int problemId, string userId);
        Task<HashSet<int>> GetUserAcceptedSubmissionIdsAsync(string userId);
        Task<Dictionary<int, SubmissionResult>> GetUserSubmissionsAsync(string userId);
        Task<bool> IsUserAuthorizedToViewSubmission (string userId, int submissionId);

    }

}
