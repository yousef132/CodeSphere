using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
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
        private readonly IUnitOfWork unitOfWork;

        public GetProblemTestCasesByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public Task<Response> Handle(GetProblemTestCasesByIdQuery request, CancellationToken cancellationToken)
        {
            var TestCases = unitOfWork.Repository<Domain.Models.Entities.Testcase>().GetByIdAsync(request.ProblemId);
            if (TestCases is null)
            {
                return Response.FailureAsync("Problem not found");

            }
            return Response.SuccessAsync(TestCases, "TestCases fetched successfully");
        }
    }
}
