using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Repositores
{
    public interface ISubmissionRepository
    {
        Task<IQueryable<Submit>> GetAllSubmissions(Guid problemId, string userId);
    }
}
