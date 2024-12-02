using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var problems = await _unitOfWork.ElasticSearchRepository.SearchProblemsAsync(request.ProblemName, request.TopicsIds, request.Difficulty);
            
            if(problems.IsNullOrEmpty())
            {
                return await Response.FailureAsync("No Problems Found", HttpStatusCode.NotFound);
            }

            var responses = new List<GetAllQueryResponse>();

            foreach (var problem in problems)
            {
                var submission =  _unitOfWork.SubmissionRepository.GetSolvedSubmissions(problem.Id, request.UserId);
                var x = _mapper.Map<GetAllQueryResponse>(problem);
                x.IsSolved = !submission.IsNullOrEmpty();
                responses.Add(x);
            }

            return await Response.SuccessAsync(responses, "Problems Found", HttpStatusCode.OK);
        }

    }
}
