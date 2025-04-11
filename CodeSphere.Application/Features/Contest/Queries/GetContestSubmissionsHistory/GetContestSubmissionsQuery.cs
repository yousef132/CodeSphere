using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Contest.Queries.GetContestSubmissionsHistory
{
    public class GetContestSubmissionsQuery : IRequest<Response>
    {
        public int ContestId { get; set; }


        public GetContestSubmissionsQuery(int contestId)
        {
            ContestId = contestId;
        }
    }

    public class GetContestSubmissionsQueryResponse
    {
        public int Id { get; set; }
        public string ProblemName { get; set; }
        public decimal Time { get; set; }
        public DateTime SubmissionDate { get; set; }
        public SubmissionResult Result { get; set; }
        public Language Language { get; set; }
    }

    public class GetContestSubmissionsQueryHandler : IRequestHandler<GetContestSubmissionsQuery, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private string? userId;

        public GetContestSubmissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
            var user = contextAccessor.HttpContext?.User;
            userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(GetContestSubmissionsQuery request, CancellationToken cancellationToken)
        {

            var contest = await unitOfWork.Repository<Domain.Models.Entities.Contest>().GetByIdAsync(request.ContestId);
            if (contest == null)
                return await Response.FailureAsync("Contest Not Found", System.Net.HttpStatusCode.NotFound);


            var submission = await unitOfWork.SubmissionRepository.GetUserContestSubmissions(request.ContestId, userId);

            var mapppedSubmissions = mapper.Map<List<GetContestSubmissionsQueryResponse>>(submission);

            return await Response.SuccessAsync(mapppedSubmissions, "Contest Submissions fetched successfully", System.Net.HttpStatusCode.OK);

        }
    }


}
