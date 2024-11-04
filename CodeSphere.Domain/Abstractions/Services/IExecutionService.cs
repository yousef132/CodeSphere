using CodeSphere.Application.Features.Problem.Commands.SolveProblem;
using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IExecutionService
    {
        Task<List<TestCaseRunResult>> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit);
    }
}
