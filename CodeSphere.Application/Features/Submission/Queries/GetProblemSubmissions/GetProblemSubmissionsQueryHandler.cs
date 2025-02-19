using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsQueryHandler : IRequestHandler<GetProblemSubmissionsQuery, Response>
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public GetProblemSubmissionsQueryHandler(IMapper mapper,
            ISubmissionRepository submissionRepository,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _submissionRepository = submissionRepository;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<Response> Handle(GetProblemSubmissionsQuery request, CancellationToken cancellationToken)
        {

            var problem = await unitOfWork.Repository<Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);

            if (problem is null)
                return await Response.FailureAsync( "Problem Not Found", HttpStatusCode.NotFound);

            var user =await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return await Response.FailureAsync( "user Not Found", HttpStatusCode.NotFound);

            var submissions = await _submissionRepository.GetAllSubmissions(request.ProblemId, request.UserId);
            var submissionsList = submissions.ToList();
            if (submissionsList.Count() == 0)
                return await Response.SuccessAsync(null, "No Submissions", HttpStatusCode.NoContent);

            var mappedSubmissions = _mapper.Map<List<GetProblemSubmissionsResponse>>(submissionsList);

            return await Response.SuccessAsync(mappedSubmissions, "Submissions fetched successfully", HttpStatusCode.Found);
        }
    }
}
