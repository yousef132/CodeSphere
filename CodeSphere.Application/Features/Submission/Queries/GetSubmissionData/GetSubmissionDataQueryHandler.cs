using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Submission.Queries.GetSubmissionData
{
    public class GetSubmissionDataQueryHandler : IRequestHandler<GetSubmissionDataQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private string UserId;
        public GetSubmissionDataQueryHandler(IUnitOfWork unitOfWork, IMapper mapper ,IHttpContextAccessor httpContextAccessor
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.contextAccessor = httpContextAccessor;

            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<Response> Handle(GetSubmissionDataQuery request, CancellationToken cancellationToken)
        {
            var submission = await _unitOfWork.Repository<Submit>().GetByIdAsync(request.SubmissionId);
            if (submission == null)
                return await Response.FailureAsync("Submission not found", System.Net.HttpStatusCode.NotFound);

            var result = await _unitOfWork.SubmissionRepository.IsUserAuthorizedToViewSubmission(UserId, request.SubmissionId);
            
            if (!result)
                return await Response.FailureAsync("You are not authorized to view this submission", System.Net.HttpStatusCode.Unauthorized);

            var mappedSub = _mapper.Map<GetSubmissionDataQueryResponse>(submission);
            return await Response.SuccessAsync(mappedSub, "Submission fetched successfully", System.Net.HttpStatusCode.OK);
        }
    }
}
