﻿using AutoMapper;
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

            var (problems, totalNumberOfPages) = await _unitOfWork.ElasticSearchRepository.SearchProblemsAsync(
                request.ProblemName,
                request.Topics,
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
                        .Where(p => allSubmissions.Any(s => s.Key == p.Id && s.Value != SubmissionResult.Accepted))
                        .ToList();
                }
                else
                {
                    mappedProblems = mappedProblems
                        .Where(p => !allSubmissions.Any(s => s.Key == p.Id))
                        .ToList();
                }

                totalNumberOfPages = (int)Math.Ceiling(mappedProblems.Count / (double)request.PageSize);

                return await Response.SuccessAsync(new { mappedProblems, totalNumberOfPages }, "Problems Found", HttpStatusCode.OK);
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                var acceptedSubmissions = await _unitOfWork.SubmissionRepository.GetUserAcceptedSubmissionIdsAsync(UserId); // get all accepted submissions for this user
                foreach (var problem in mappedProblems)
                    problem.IsSolved = acceptedSubmissions.Contains(problem.Id);
            }

            return await Response.SuccessAsync(new { mappedProblems, totalNumberOfPages }, "Problems Found", HttpStatusCode.OK);

        }

    }
}
