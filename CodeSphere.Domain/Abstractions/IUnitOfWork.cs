using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        IElasticSearchRepository ElasticSearchRepository { get; }
        IProblemRepository ProblemRepository { get; }
        ISubmissionRepository SubmissionRepository { get; }
        Task<int> CompleteAsync();
    }
}
