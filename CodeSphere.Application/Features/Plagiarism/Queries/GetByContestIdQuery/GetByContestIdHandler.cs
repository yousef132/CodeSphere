using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Plagiarism.Queries.GetByContestIdQuery
{
    public class GetByContestIdHandler : IRequestHandler<GetByContestIdQuery, Response>
    {
        IPlagiarismService plagiarismService;
        public GetByContestIdHandler(IPlagiarismService plagiarismService)
        {
            this.plagiarismService = plagiarismService;
        }
        public async Task<Response> Handle(GetByContestIdQuery request, CancellationToken cancellationToken)
        {
            var cases = await plagiarismService.GetPlagiarismCases(request.ContestId, request.ProblemIds, request.threshold);
            if (cases.IsNullOrEmpty())
            {
                return await Response.FailureAsync("No cases found");
            }
            return await Response.SuccessAsync(cases);
        }
    }
}
