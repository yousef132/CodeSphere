namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface IContestRepository
    {
        Task<IReadOnlyList<Domain.Models.Entities.Problem>> GetContestProblemsByIdAsync(int contestId);
        Task<IReadOnlyList<(Domain.Models.Entities.Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId);
    }
}
