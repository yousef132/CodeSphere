using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Submission.Queries.GetSubmissionData
{
    public sealed class GetSubmissionDataQuery : IRequest<Response>
    {
        public int SubmissionId { get; set; }
        public string UserId { get; set; }


    }
}
