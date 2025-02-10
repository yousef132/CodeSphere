using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

        public async Task<HashSet<int>> GetUserAcceptedSubmissionIdsAsync(string userId)
        {
            List<int> problemIds = await _context.Submits
                        .Where(s => s.UserId == userId && s.Result == SubmissionResult.Accepted)
                        .Select(s => s.ProblemId).ToListAsync();

            return new HashSet<int>(problemIds);
        }

        public async Task<Dictionary<int, SubmissionResult>> GetUserSubmissionsAsync(string userId)
        {

            // problemId: Status


            var submissions = await _context.Submits
            .Where(s => s.UserId == userId)
            .ToListAsync();

            var result = new Dictionary<int, SubmissionResult>();

            foreach (var submission in submissions)
            {
                if (!result.ContainsKey(submission.ProblemId) || submission.Result == SubmissionResult.Accepted)
                {
                    result[submission.ProblemId] = submission.Result;
                }
            }

            return result;

        }
    }

}
