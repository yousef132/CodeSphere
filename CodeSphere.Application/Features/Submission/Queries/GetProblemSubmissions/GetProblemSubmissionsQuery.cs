using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsQuery : IRequest<Response>
    {
        public GetProblemSubmissionsQuery(int problemId)
        {
            this.ProblemId = problemId;
        }
        public int ProblemId { get; set; }
    }
}
