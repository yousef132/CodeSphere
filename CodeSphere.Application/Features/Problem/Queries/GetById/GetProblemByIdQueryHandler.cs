using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
    public class GetProblemByIdQueryHandler : IRequestHandler<GetProblemByIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private string UserId;
        public GetProblemByIdQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.contextAccessor = contextAccessor;

            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(GetProblemByIdQuery request, CancellationToken cancellationToken)
        {
            // Get the Problem Details from Repository
            var problem = await _unitOfWork.ProblemRepository.GetProblemIncludingContestAndTestcases(request.ProblemId);



            // Check if Problem was not Found
            if (problem is null)
                return await Response.FailureAsync("Problem not Found", HttpStatusCode.NotFound);


            if (problem.Contest.ContestStatus == ContestStatus.Upcoming)
                return await Response.FailureAsync("Contest is Upcoming", HttpStatusCode.Forbidden);

            if (problem.Contest.ContestStatus == ContestStatus.Running)
            {
                // return bad request if not registered 
                var isRegistered = await _unitOfWork.UserContestRepository.IsRegistered(problem.ContestId, UserId);
                if (isRegistered == null)
                    return await Response.FailureAsync("You are not registered in this contest", HttpStatusCode.Forbidden);
            }

            problem.Testcases = problem.Testcases?.Take(3).ToList() ?? [];

            // Map to the response 
            var response = _mapper.Map<GetByIdQueryResponse>(problem);

            // Populate Accepted and Submissions counts
            response.Accepted = _unitOfWork.ProblemRepository.GetAcceptedProblemCount(request.ProblemId);

            response.Submissions = _unitOfWork.ProblemRepository.GetSubmissionsProblemCount(request.ProblemId);

            if (!string.IsNullOrEmpty(UserId))
            {
                // Check if the user has solved the problem
                response.IsSolved = _unitOfWork.ProblemRepository.CheckUserSolvedProblem(
                    request.ProblemId, UserId, cancellationToken);
            }

            // Return the success response
            return await Response.SuccessAsync(response, "Problem Found", HttpStatusCode.OK);
        }
    }
}
