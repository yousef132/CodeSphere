using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Testcases.Queries.GetTestCasesByProblemId
{
    public sealed record GetTestCasesByProblemIdQuerey(
        int ProblemId
    ) : IRequest<Response>;
}
