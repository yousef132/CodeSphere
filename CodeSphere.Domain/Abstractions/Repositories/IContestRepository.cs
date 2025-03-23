using CodeSphere.Domain.Responses.Contest;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface IContestRepository
    {
        Task<IReadOnlyList<Domain.Models.Entities.Problem>> GetContestProblemsByIdAsync(int contestId);
        Task<IReadOnlyList<(Domain.Models.Entities.Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId);

        Task<IReadOnlyList<StandingDto>> GetContestStanding(int contestId);
    }
}
