using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Infrastructure.Context;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<Submit>> GetAllSubmissions(Guid problemId, string userId)
         => _context.Submits.Where(x => x.ProblemId == problemId && x.UserId == userId);

    }
}
