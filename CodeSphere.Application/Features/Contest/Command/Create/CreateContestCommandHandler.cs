using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Contest.Command.Create
{
    public class CreateContestCommandHandler : IRequestHandler<CreateContestCommand, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private string UserId;
        public CreateContestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;

            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }




        public async Task<Response> Handle(CreateContestCommand request, CancellationToken cancellationToken)
        {
            var contest = mapper.Map<Domain.Models.Entities.Contest>(request);
            contest.ProblemSetterId = UserId;
            await unitOfWork.Repository<Domain.Models.Entities.Contest>().AddAsync(contest);
            await unitOfWork.CompleteAsync();
            return await Response.SuccessAsync(contest.Id, "Contest Created Successfully", System.Net.HttpStatusCode.Created);
        }
    }
}
