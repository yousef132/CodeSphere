using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsQuery : IRequest<Response>
    {
        public GetProblemSubmissionsQuery(int problemId, string UserId)
        {
            this.ProblemId = problemId;
            this.UserId = UserId;   
        }
        public string UserId { get; set; }
        public int ProblemId { get; set; }
    }
}
