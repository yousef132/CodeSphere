using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class RankUpService : IRankUpService
    {
        private readonly IEmailService emailService;
        private readonly IUnitOfWork unitOfWork;

        public RankUpService(IEmailService emailService,IUnitOfWork unitOfWork)
        {
            this.emailService = emailService;
            this.unitOfWork = unitOfWork;
        }
        public Task InceaseUserRank(string userId, int problemId)
        {
            throw new NotImplementedException();
        }

        public Task LevelUpUserRank(string userId, ContestPoints points)
        {
            throw new NotImplementedException();
        }
    }
}
