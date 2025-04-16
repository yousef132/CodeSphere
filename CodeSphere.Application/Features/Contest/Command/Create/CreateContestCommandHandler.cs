using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.DTOs;
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
        private readonly string UserId;
        public CreateContestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

            var user = contextAccessor.HttpContext?.User;
            UserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public async Task<Response> Handle(CreateContestCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(UserId))
                return await Response.FailureAsync("User must be authenticated", System.Net.HttpStatusCode.Unauthorized);

            var contest = mapper.Map<Domain.Models.Entities.Contest>(request);
            contest.ProblemSetterId = UserId;
            await unitOfWork.Repository<Domain.Models.Entities.Contest>().AddAsync(contest);
            await unitOfWork.CompleteAsync();

            var responseDto = new ContestResponseDto
            {
                Id = contest.Id,
                Name = contest.Name
            };

            return await Response.SuccessAsync(responseDto, "Contest Created Successfully", System.Net.HttpStatusCode.Created);
        }
    }
}