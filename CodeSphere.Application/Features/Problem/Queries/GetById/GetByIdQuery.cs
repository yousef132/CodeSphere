using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
    public class GetByIdQuery : IRequest<Response>
    {
        public int ProblemId { get; set; }
        public string UserId { get; set; }

        public GetByIdQuery(int problemId, string userId)
        {
            ProblemId = problemId;
            UserId = userId;
        }
    }


}
