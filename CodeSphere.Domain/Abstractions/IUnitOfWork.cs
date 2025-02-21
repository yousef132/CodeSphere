using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        IElasticSearchRepository ElasticSearchRepository { get; }
        IContestRepository ContestRepository { get; }
        IUserContestRepository UserContestRepository { get; }
        IProblemRepository ProblemRepository { get; }
        ISubmissionRepository SubmissionRepository { get; }
        ITopicRepository TopicRepository { get; }
        Task<int> CompleteAsync();
    }
}
