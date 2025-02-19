using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class ContestRepository : IContestRepository
    {
        private readonly ApplicationDbContext context;

        public ContestRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<IReadOnlyList<Problem>> GetContestProblemsByIdAsync(int contestId)
        {
            var problems = await context.Problems.Where(x => x.ContestId == contestId)
                 .ToListAsync();
            return problems;
        }
    }
}
