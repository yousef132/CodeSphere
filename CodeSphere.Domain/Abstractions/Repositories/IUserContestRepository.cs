using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface IUserContestRepository
    {
        Task<UserContest?> GetUserContest(string userId, int contestId);

        Task<bool> RegisterInContest(UserContest registration);
        Task<UserContest?> IsRegistered(int contestId, string userId);

    }
}
