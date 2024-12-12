using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllProblemsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private string UserId;

        public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.contextAccessor = contextAccessor;
            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(GetAllProblemsQuery request, CancellationToken cancellationToken)
        {
            var problems = await _unitOfWork.ElasticSearchRepository.SearchProblemsAsync(request.ProblemName, request.TopicsIds, request.Difficulty);

            if (problems.IsNullOrEmpty())
                return await Response.FailureAsync("No Problems Found", HttpStatusCode.NotFound);

            var mappedProblems = _mapper.Map<IReadOnlyList<GetAllQueryResponse>>(problems);


            if (!string.IsNullOrEmpty(UserId))
            {
                var submissions = await _unitOfWork.SubmissionRepository.GetUserAcceptedSubmissionIdsAsync(UserId); // get all accepted submissions for this user
                foreach (var problem in mappedProblems)
                    problem.IsSolved = submissions.Contains(problem.Id);
            }
            return await Response.SuccessAsync(mappedProblems, "Problems Found", HttpStatusCode.OK);

        }

    }
}
