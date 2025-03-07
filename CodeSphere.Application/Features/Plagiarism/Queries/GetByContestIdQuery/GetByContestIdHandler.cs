using AutoMapper;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.DTOs;
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
        IMapper mapper;
        public GetByContestIdHandler(IPlagiarismService plagiarismService, IMapper mapper)
        {
            this.plagiarismService = plagiarismService;
            this.mapper = mapper;
        }
        public async Task<Response> Handle(GetByContestIdQuery request, CancellationToken cancellationToken)
        {
            var cases = await plagiarismService.GetPlagiarismCases(request.ContestId, request.ProblemIds, request.threshold);
            if (cases.IsNullOrEmpty())
            {
                return await Response.FailureAsync("No cases found");
            }
            var mappedCases = cases.Select(c => new GetByContestIdResponse
            {
                FirstSubmission = mapper.Map<SubmissionDTO>(c.FirstSubmission),
                SecondSubmission = mapper.Map<SubmissionDTO>(c.SecondSubmission),
                ProblemId = c.ProblemId,
                Similarity = c.Similarity,
                
            });
            return await Response.SuccessAsync(mappedCases);
        }
    }
}
