using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetProblemTestCasesById
{
    public class GetProblemTestCasesByIdQueryHandler : IRequestHandler<GetProblemTestCasesByIdQuery, Response>
    {
        private readonly IMapper mapper;
        private readonly IProblemRepository _problemRepository;


        public GetProblemTestCasesByIdQueryHandler(IMapper mapper,
            IProblemRepository problemRepository)
        {
            this.mapper = mapper;
            _problemRepository = problemRepository;
        }
        public async Task<Response> Handle(GetProblemTestCasesByIdQuery request, CancellationToken cancellationToken)
        {
            var TestCases = await _problemRepository.GetTestCasesByProblemId(request.ProblemId);
            if (TestCases.IsNullOrEmpty())
            {
                return await Response.FailureAsync("Problem not found");
            }
            return await Response.SuccessAsync(TestCases, "TestCases fetched successfully");
        }
    }
}
