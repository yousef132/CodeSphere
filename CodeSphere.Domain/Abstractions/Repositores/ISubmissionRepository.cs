using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Abstractions.Repositores
{
    public interface ISubmissionRepository
    {
        Task<IEnumerable<Submit>> GetAllSubmissions(Guid problemId, string userId);
    }
}
