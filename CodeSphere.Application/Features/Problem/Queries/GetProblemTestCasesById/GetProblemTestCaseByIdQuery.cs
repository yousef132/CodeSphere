using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Queries.GetProblemTestCasesById
{
    public sealed record GetProblemTestCasesByIdQuery(
        int ProblemId
    ) : IRequest<Response>;
}
