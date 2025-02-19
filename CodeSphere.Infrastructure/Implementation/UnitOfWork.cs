using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Premitives;
using CodeSphere.Infrastructure.Context;
using CodeSphere.Infrastructure.Implementation.Repositories;
using System.Collections;

namespace CodeSphere.Infrastructure.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private Hashtable _repositories;
        public IElasticSearchRepository ElasticSearchRepository { get; }

        public IProblemRepository ProblemRepository { get; }
        public ISubmissionRepository SubmissionRepository { get; }
        public ITopicRepository TopicRepository { get; }
        public IBlogRepository BlogRepository { get; }
        public UnitOfWork(ApplicationDbContext context,
            IElasticSearchRepository elasticSearchRepository,
            IProblemRepository problemRepository,
            ISubmissionRepository submissionRepository,
            ITopicRepository topicRepository,
            IBlogRepository blogRepository)
        {
            this.context = context;
            _repositories = new Hashtable();
            ElasticSearchRepository = elasticSearchRepository;
            ProblemRepository = problemRepository;
            SubmissionRepository = submissionRepository;
            TopicRepository = topicRepository;
            BlogRepository= blogRepository; 
        }
        


        public Task<int> CompleteAsync()
            => context.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => context.DisposeAsync();

        // create repository per request  
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // if repository<order> => key = order
            var key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(key))
            {
                var repo = new GenericRepository<TEntity>(context);

                _repositories.Add(key, repo);
            }

            return _repositories[key] as IGenericRepository<TEntity>;
        }
    }
}
