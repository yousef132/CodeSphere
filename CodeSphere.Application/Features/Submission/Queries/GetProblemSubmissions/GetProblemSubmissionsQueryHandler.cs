using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsQueryHandler : IRequestHandler<GetProblemSubmissionsQuery, Response>
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUnitOfWork unitOfWork;
        private string UserId;
        public GetProblemSubmissionsQueryHandler(IMapper mapper,
            ISubmissionRepository submissionRepository,
            IHttpContextAccessor contextAccessor,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _submissionRepository = submissionRepository;
            this.contextAccessor = contextAccessor;
            this.unitOfWork = unitOfWork;
            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<Response> Handle(GetProblemSubmissionsQuery request, CancellationToken cancellationToken)
        {

            var problem = await unitOfWork.Repository<Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);

            if (problem is null)
                return await Response.SuccessAsync(null, "Problem Not Found", HttpStatusCode.NotFound);

            var submissions = await _submissionRepository.GetAllSubmissions(request.ProblemId, UserId);

            if (submissions.IsNullOrEmpty())
                return await Response.SuccessAsync(null, "Not Submissions", HttpStatusCode.NoContent);

            var mappedSubmissions = _mapper.Map<IQueryable<Submit>, IQueryable<GetProblemSubmissionsResponse>>(submissions);

            return await Response.SuccessAsync(mappedSubmissions, "Submissions fetched successfully", HttpStatusCode.Found);
        }
    }
}
