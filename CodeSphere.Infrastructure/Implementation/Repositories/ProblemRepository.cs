using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Infrastructure.Context;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class ProblemRepository : IProblemRepository
    {
        private readonly ApplicationDbContext _context;
        public ProblemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Testcase> GetTestCasesByProblemId(int problemId)
           => _context.Set<Testcase>().Where(x => x.ProblemId == problemId);
    }
}
