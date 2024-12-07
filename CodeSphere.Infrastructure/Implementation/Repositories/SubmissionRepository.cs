using CodeSphere.Domain.Abstractions.Repositories;
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
        public async Task<IQueryable<Submit>> GetAllSubmissions(int problemId, string userId)
         => _context.Submits.Where(x => x.ProblemId == problemId && x.UserId == userId);

        public IQueryable<Submit> GetSolvedSubmissions(int problemId, string userId)
        {
            return _context.Submits.Where(x => x.ProblemId == problemId && x.UserId == userId && x.Result == SubmissionResult.Accepted);
        }

        public IQueryable<Submit> GetUserSolvedProblems(string userId)
            => _context.Submits.Where(s => s.UserId == userId && s.Result == SubmissionResult.Accepted);
    }

}
