using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsQuery : IRequest<Response>
    {
        public int ProblemId { get; set; }
        public string UserId { get; set; }
    }
}
