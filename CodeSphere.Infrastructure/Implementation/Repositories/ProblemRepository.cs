using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class ProblemRepository : IProblemRepository
    {
        private readonly ApplicationDbContext _context;
        public ProblemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Testcase>> GetTestCasesByProblemId(Guid problemId)
           => await _context.Set<Testcase>().Where(x => x.ProblemId == problemId).ToListAsync();
    }
}
