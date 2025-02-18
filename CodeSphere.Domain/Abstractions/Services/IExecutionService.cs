using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Requests;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IExecutionService
    {
        Task<object> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit);
        Task<object> ExecuteCodeAsync(string code, Language language, List<CustomTestcaseDto> testcases, decimal runTimeLimit);
    }
}
