using AutoMapper;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsQueryHandler : IRequestHandler<GetProblemSubmissionsQuery, Response>
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionRepository _submissionRepository;

        public GetProblemSubmissionsQueryHandler(IMapper mapper, ISubmissionRepository submissionRepository)
        {
            _mapper = mapper;
            _submissionRepository = submissionRepository;
        }

        public async Task<Response> Handle(GetProblemSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var submissions = await _submissionRepository.GetAllSubmissions(request.ProblemId, request.UserId);
            if(submissions.IsNullOrEmpty())
            {
                return await Response.FailureAsync("Submissions not found");
            }
            var mappedSubmissions = _mapper.Map<IEnumerable<Submit>, IEnumerable<GetProblemSubmissionsResponse>>(submissions);
            return await Response.SuccessAsync(mappedSubmissions, "Submissions fetched successfully");
        }
    }
}
