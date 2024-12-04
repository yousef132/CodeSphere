using CodeSphere.Domain.Abstractions.Repositories;
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

		public async Task<Problem?> GetProblemDetailsAsync(int problemId, CancellationToken cancellationToken = default)
		=> await _context.Set<Problem>().Include(x => x.Testcases).Take(3).Include(y => y.ProblemTopics).ThenInclude(x=>x.Topic).FirstOrDefaultAsync(x=>x.Id == problemId);
		
		
		
	
		
		public int GetAcceptedProblemCount(int problemId, CancellationToken cancellationToken = default)
		=> _context.Set<Submit>().Where(P => P.ProblemId == problemId && P.Result == SubmissionResult.Accepted).Count();	



		public int GetSubmissionsProblemCount(int problemId, CancellationToken cancellationToken = default)
				=> _context.Set<Submit>().Where(P => P.ProblemId == problemId).Count();
		public IQueryable<Testcase> GetTestCasesByProblemId(int problemId)
           => _context.Set<Testcase>().Where(x => x.ProblemId == problemId);

	}
}
