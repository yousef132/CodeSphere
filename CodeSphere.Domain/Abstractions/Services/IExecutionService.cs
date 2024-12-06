using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Requests;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IExecutionService
    {
        Task<object> ExecuteCodeAsync(string code, Language language, List<CustomTestcaseDto> testcaseDtos, decimal runTimeLimit, decimal memoryLimit);
        Task<object> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit, decimal memoryLimit);
    }
}
