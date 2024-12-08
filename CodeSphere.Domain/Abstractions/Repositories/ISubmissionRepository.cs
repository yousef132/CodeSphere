using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface ISubmissionRepository
    {
        Task<IQueryable<Submit>> GetAllSubmissions(int problemId, string userId);
        IQueryable<Submit> GetSolvedSubmissions(int problemId, string userId);
        IQueryable<Submit> GetUserAcceptedSubmissions(string userId);

    }

}
