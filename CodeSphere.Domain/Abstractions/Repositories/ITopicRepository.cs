using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface ITopicRepository
    {
        Task<List<string>> GetTopicNamesByIdsAsync(IEnumerable<int> topicIds);
        Task<List<int>> GetTopicIDsByNamesAsync(IEnumerable<string> topicsNames);

    }
}
