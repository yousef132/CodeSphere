using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IExecutionService
    {
        Task<object> ExecuteCodeAsync(string code, Language language, Testcase testCase, decimal runTimeLimit, decimal memoryLimit);
        Task<object> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit, decimal memoryLimit);
    }
}
