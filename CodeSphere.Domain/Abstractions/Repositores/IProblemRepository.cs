using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Repositores
{
    public interface IProblemRepository
    {
        IQueryable<Testcase> GetTestCasesByProblemId(int problemId);
    }
}
