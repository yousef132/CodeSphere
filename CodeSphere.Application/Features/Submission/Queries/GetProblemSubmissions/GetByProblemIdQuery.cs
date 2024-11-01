using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public sealed record GetProblemSubmissionsQuery(
         Guid ProblemId,
         string UserId
     ) : IRequest<Response>;
}
