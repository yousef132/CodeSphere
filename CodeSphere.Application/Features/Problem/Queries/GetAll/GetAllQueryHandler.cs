using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            var topicsIds = await _unitOfWork.TopicRepository
                .GetTopicIDsByNamesAsync(request.TopicsNames);


            var problems = await _unitOfWork.ElasticSearchRepository.SearchProblemsAsync(
                request.ProblemName,
                topicsIds,
                request.Difficulty,
                request.SortBy,
                request.Order,
                request.PageNumber,
                request.PageSize);

            if (problems.IsNullOrEmpty())
                return await Response.FailureAsync("No Problems Found", HttpStatusCode.NotFound);

            var mappedProblems = _mapper.Map<IReadOnlyList<GetAllQueryResponse>>(problems);


            var status = request.Status;
            if (status is not null)
            {
                var allSubmissions = await _unitOfWork.SubmissionRepository.GetUserSubmissionsAsync(UserId);
                if (status == ProblemStatus.Solved)
                {
                    mappedProblems = mappedProblems
                        .Where(p => allSubmissions.Any(s => s.Key == p.Id && s.Value == SubmissionResult.Accepted))
                        .ToList();
                } 
                else if (status == ProblemStatus.Attempted)
                {
                    mappedProblems = mappedProblems
                        .Where(p => !allSubmissions.Any(s => s.Key == p.Id))
                        .ToList();
                }
                else
                {
                    mappedProblems = mappedProblems
                        .Where(p => allSubmissions.Any(s => s.Key == p.Id && s.Value != SubmissionResult.Accepted))
                        .ToList();
                }
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                var acceptedSubmissions = await _unitOfWork.SubmissionRepository.GetUserAcceptedSubmissionIdsAsync(UserId); // get all accepted submissions for this user
                foreach (var problem in mappedProblems)
                    problem.IsSolved = acceptedSubmissions.Contains(problem.Id);
            }

            int totalNumberOfpages = Math.Max(1, mappedProblems.Count / request.PageSize);
            return await Response.SuccessAsync(new { totalNumberOfpages, mappedProblems}, "Problems Found", HttpStatusCode.OK);

        }

    }
}
