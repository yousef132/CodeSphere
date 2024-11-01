using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Abstractions.Repositores
{
    public interface IProblemRepository
    {
        Task<IEnumerable<Testcase>> GetTestCasesByProblemId(Guid problemId);
    }
}
