using AutoMapper;
using CodeSphere.Application.Features.Contest.Queries.GetContestProblems;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;

namespace CodeSphere.Application.Features.Contest.Queries
{
    public class GetContestProblemsQueryHandler : IRequestHandler<GetContestProblemsQuery, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IResponseCacheService cacheService;
        private readonly IHttpContextAccessor httpContext;
        private readonly string UserId;
        private const int CacheDurationHours = 2;

        public GetContestProblemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService cacheService, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.httpContext = httpContext;

            UserId = httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public async Task<Response> Handle(GetContestProblemsQuery request, CancellationToken cancellationToken)
        {
            var contest = await unitOfWork.Repository<Domain.Models.Entities.Contest>().GetByIdAsync(request.Id);
            if (contest == null)
                return await Response.FailureAsync("Contest Not Found", System.Net.HttpStatusCode.NotFound);

            return contest.ContestStatus switch
            {
                ContestStatus.Upcoming => await Response.FailureAsync("Contest is not started yet", System.Net.HttpStatusCode.Forbidden),
                ContestStatus.Running => await HandleRunningContest(request),
                _ => await FetchAndReturnContestProblems(request.Id)
            };
        }

        private async Task<Response> HandleRunningContest(GetContestProblemsQuery request)
        {
            var isRegistered = await unitOfWork.UserContestRepository.IsRegistered(request.Id, UserId);
            if (isRegistered == null)
                return await Response.FailureAsync("You are not registered in this contest", System.Net.HttpStatusCode.Forbidden);

            string cacheKey = GenerateCacheKeyFromRequest();
            string cachedData = await cacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var serializedData = Helper.DeserializeCollection<ContestProblemResponse>(cachedData);
                return await Response.SuccessAsync(serializedData, "Contest Problems fetched successfully", System.Net.HttpStatusCode.Found);
            }

            return await FetchAndCacheContestProblems(request.Id, cacheKey);
        }

        private async Task<Response> FetchAndReturnContestProblems(int contestId)
        {
            var dbProblems = await unitOfWork.ContestRepository.GetContestProblemsByIdAsync(contestId);
            var dbMappedProblems = mapper.Map<IReadOnlyList<ContestProblemResponse>>(dbProblems);
            return await Response.SuccessAsync(dbMappedProblems, "Contest Problems fetched successfully", System.Net.HttpStatusCode.Found);
        }

        private async Task<Response> FetchAndCacheContestProblems(int contestId, string cacheKey)
        {
            var problems = await unitOfWork.ContestRepository.GetContestProblemsByIdAsync(contestId);
            var mappedProblems = mapper.Map<IReadOnlyList<ContestProblemResponse>>(problems);

            await cacheService.CacheResponseAsync(cacheKey, mappedProblems, TimeSpan.FromHours(CacheDurationHours));

            return await Response.SuccessAsync(mappedProblems, "Contest Problems fetched successfully", System.Net.HttpStatusCode.Found);
        }

        private string GenerateCacheKeyFromRequest()
        {
            var request = httpContext.HttpContext?.Request;
            if (request == null) return string.Empty;

            var keyBuilder = new StringBuilder(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
