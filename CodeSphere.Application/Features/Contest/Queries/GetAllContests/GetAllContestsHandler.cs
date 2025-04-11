using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Contest.Queries.GetAllContests
{
    public class GetAllContestsHandler : IRequestHandler<GetAllContestsQuery, Response>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor contextAccessor;
        private string? userId;

        public GetAllContestsHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.contextAccessor = contextAccessor;
            var user = contextAccessor.HttpContext?.User;

            userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //userId = contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public async Task<Response> Handle(GetAllContestsQuery request, CancellationToken cancellationToken)
        {
            var contests = await unitOfWork.ContestRepository.GetAllContestWithRegisteredUserAsync(userId);
            //var contests = await unitOfWork.ContestRepository.GetAllContestWithRegisteredUserAsync();

            var mappedContests = mapper.Map<IReadOnlyList<GetAllContestsQueryResponse>>(contests);

            return await Response.SuccessAsync(mappedContests);

        }
    }
}
