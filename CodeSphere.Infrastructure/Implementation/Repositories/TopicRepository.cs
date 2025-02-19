using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<string>> GetTopicNamesByIdsAsync(IEnumerable<int> topicIds)
        {
            return await _context.Topics
                .Where(x => topicIds.Contains(x.Id))
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<int>> GetTopicIDsByNamesAsync(IEnumerable<string> topicsNames)
        {
            return await _context.Topics
                .Where(x => topicsNames.Contains(x.Name))
                .Select(x => x.Id)
                .ToListAsync();
        }
    }
}
