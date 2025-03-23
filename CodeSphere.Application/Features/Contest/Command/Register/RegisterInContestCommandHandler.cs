using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Contest.Command.Register
{
    public class RegisterInContestCommandHandler : IRequestHandler<RegisterInContestCommand, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor contextAccessor;
        private string UserId;
        public RegisterInContestCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.contextAccessor = contextAccessor;

            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(RegisterInContestCommand request, CancellationToken cancellationToken)
        {
            var contest = await unitOfWork.Repository<Domain.Models.Entities.Contest>().GetByIdAsync(request.Id);
            if (contest != null)
                return await Response.FailureAsync("No Contest Found", HttpStatusCode.NotFound);

            if (contest.ContestStatus == ContestStatus.Ended)
                return await Response.FailureAsync("Contest Ended", HttpStatusCode.Forbidden);

            var isRegistered = await unitOfWork.UserContestRepository.IsRegistered(request.Id, UserId);
            if (isRegistered != null)
                return await Response.FailureAsync("Already registered in this contest", System.Net.HttpStatusCode.BadRequest);



            var registration = new UserContest
            {
                UserId = UserId,
                ContestId = request.Id,
            };

            var result = await unitOfWork.UserContestRepository.RegisterInContest(registration);

            return result ? await Response.SuccessAsync(null, message: "registered successfully", HttpStatusCode.OK)
                          : await Response.FailureAsync(message: "Failed To Register", HttpStatusCode.InternalServerError);


        }
    }
}
