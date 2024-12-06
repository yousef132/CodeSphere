using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface IProblemRepository
    {
        IQueryable<Testcase> GetTestCasesByProblemId(int problemId);

        Task<Problem?> GetProblemDetailsAsync(int problemId, CancellationToken cancellationToken = default);  

        int GetAcceptedProblemCount(int problemId, CancellationToken cancellationToken = default);
        int GetSubmissionsProblemCount(int problemId, CancellationToken cancellationToken = default);

        public bool CheckUserSolvedProblem(int problemId, string userId, CancellationToken cancellationToken = default);

	}
}
