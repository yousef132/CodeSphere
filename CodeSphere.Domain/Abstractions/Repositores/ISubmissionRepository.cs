using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Repositores
{
    public interface ISubmissionRepository
    {
        Task<IQueryable<Submit>> GetAllSubmissions(int problemId, string userId);
    }

}
