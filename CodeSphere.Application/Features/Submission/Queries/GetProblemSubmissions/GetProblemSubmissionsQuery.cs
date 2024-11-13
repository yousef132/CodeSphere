using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public sealed record GetProblemSubmissionsQuery(
         int ProblemId,
         string UserId
     ) : IRequest<Response>;
}
