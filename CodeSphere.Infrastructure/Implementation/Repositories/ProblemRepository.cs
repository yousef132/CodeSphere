using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
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
        => await _context.Set<Problem>().Include(x => x.Testcases).Include(y => y.ProblemTopics).ThenInclude(x => x.Topic).FirstOrDefaultAsync(x => x.Id == problemId);


        public int GetAcceptedProblemCount(int problemId, CancellationToken cancellationToken = default)
        => _context.Set<Submit>().Where(P => P.ProblemId == problemId && P.Result == SubmissionResult.Accepted).Count();



        public int GetSubmissionsProblemCount(int problemId, CancellationToken cancellationToken = default)
        => _context.Set<Submit>().Where(P => P.ProblemId == problemId).Count();

        public bool CheckUserSolvedProblem(int problemId, string userId, CancellationToken cancellationToken = default)
        => _context.Set<Submit>().Any(P => P.ProblemId == problemId && P.UserId == userId && P.Result == SubmissionResult.Accepted);



        public IQueryable<Testcase> GetTestCasesByProblemId(int problemId)
        => _context.Set<Testcase>().Where(x => x.ProblemId == problemId).AsNoTracking();


        public async Task<Problem?> GetProblemIncludingContestAndTestcases(int problemId)
          => await _context.Set<Problem>()
                     .Include(p => p.Contest)
                     .Include(p => p.Testcases)
                     .AsNoTracking()
                     .FirstOrDefaultAsync(p => p.Id == problemId);


    }
}
