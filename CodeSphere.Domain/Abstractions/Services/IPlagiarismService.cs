using CodeSphere.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IPlagiarismService
    {
        Task<IEnumerable<PlagiarismCaseDTO>> GetPlagiarismCases(int contestId, List<int> exceptProblemIds, int threshold);
    }
}
