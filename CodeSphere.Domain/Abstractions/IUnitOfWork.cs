using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        IElasticSearchRepository ElasticSearchRepository { get; }
        IContestRepository ContestRepository { get; }
        IProblemRepository ProblemRepository { get; }
        ISubmissionRepository SubmissionRepository { get; }
        ITopicRepository TopicRepository { get; }
        IBlogRepository BlogRepository { get; } 
        Task<int> CompleteAsync();
    }
}
