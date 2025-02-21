using AutoMapper;
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
    public class GetContestQueryHandler : IRequestHandler<GetContestProblemsQuery, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IResponseCacheService cacheService;
        private readonly IHttpContextAccessor httpContext;
        private string UserId;
        public GetContestQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IResponseCacheService cacheService, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.httpContext = httpContext;
            this.httpContext = httpContext;

            var user = httpContext.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(GetContestProblemsQuery request, CancellationToken cancellationToken)
        {

            // upcoming => forbidden
            // running => check cach first 
            // past => ok

            var contest = await unitOfWork.Repository<Domain.Models.Entities.Contest>().GetByIdAsync(request.Id);
            if (contest == null)
                return await Response.FailureAsync("Contest Not Found", System.Net.HttpStatusCode.NotFound);

            // upcoming => forbidden
            if (contest.ContestStatus == ContestStatus.Upcoming)
                return await Response.FailureAsync("Contest is not started yet", System.Net.HttpStatusCode.Forbidden);

            // running => check cach first 
            if (contest.ContestStatus == ContestStatus.Running)
            {
                var isRegistered = await unitOfWork.UserContestRepository.IsRegistered(request.Id, UserId);
                if (isRegistered == null)
                    return await Response.FailureAsync("You are not registered in this contest", System.Net.HttpStatusCode.Forbidden);

                // check cache
                string cacheKey = GenerateCacheKeyFromRequest();
                string cachedData = await cacheService.GetCachedResponseAsync(cacheKey);

                // cache hit => return cached data
                if (cachedData != null)
                {
                    var serializedData = Helper.DeserializeCollection<ContestProblemResponse>(cachedData);
                    return await Response.SuccessAsync(serializedData, "Contest Problems fetched successfully", System.Net.HttpStatusCode.Found);
                }
                else
                {
                    // cache miss => get from db
                    var problems = await unitOfWork.ContestRepository.GetContestProblemsByIdAsync(request.Id);
                    var mappedProblem = mapper.Map<IReadOnlyList<ContestProblemResponse>>(problems);

                    // save to cache
                    await cacheService.CacheResponseAsync(cacheKey, mappedProblem, TimeSpan.FromHours(2));

                    return await Response.SuccessAsync(mappedProblem, "Contest Problems fetched successfully", System.Net.HttpStatusCode.Found);

                }
            }

            // past => get from db 
            var dbProblems = await unitOfWork.ContestRepository.GetContestProblemsByIdAsync(request.Id);
            var dbMappedProblem = mapper.Map<IReadOnlyList<ContestProblemResponse>>(dbProblems);

            return await Response.SuccessAsync(dbMappedProblem, "Contest Problems fetched successfully", System.Net.HttpStatusCode.Found);
        }

        private string GenerateCacheKeyFromRequest()
        {
            // key : unique for each request so generate it from request
            // generate it from URL Path + Query String 
            var request = httpContext.HttpContext.Request;
            StringBuilder keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path);// api/product

            //pageIndex=2&
            //pageSize=6&
            //sort=name

            // Key : api/product|pageIndex-2|pageSize-6|sort-name
            // Ordered by key to handle cases when the order of query string parameters changes but the values remain the same.

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }


}
