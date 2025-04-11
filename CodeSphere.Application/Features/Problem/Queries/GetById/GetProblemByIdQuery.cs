using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
    public class GetProblemByIdQuery : IRequest<Response>
    {
        public int ProblemId { get; set; }

        public GetProblemByIdQuery(int problemId)
        {
            ProblemId = problemId;
        }
    }


}
