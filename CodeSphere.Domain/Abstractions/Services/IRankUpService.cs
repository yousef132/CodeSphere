using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IRankUpService
	{
		Task InceaseUserRank(string userId,int problemId);

		Task LevelUpUserRank(string userId,ContestPoints points);
    }
}
