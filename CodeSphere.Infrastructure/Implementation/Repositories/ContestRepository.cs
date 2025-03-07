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

        public Task<IReadOnlyList<(Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        //public async Task<IReadOnlyList<(Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId)
        //    => await context.Contests.Include(c => c.Registrations.Where(r => r.UserId == userId)).Select(c=>
        //    {
        //        var isRegistered = c.Registrations.Any();
        //        return (c, isRegistered);
        //    }).ToListAsync();

        public async Task<IReadOnlyList<Problem>> GetContestProblemsByIdAsync(int contestId)

             => await context.Problems.Where(x => x.ContestId == contestId).ToListAsync();


    }
}
