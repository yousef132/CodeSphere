using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Submission.Queries.GetSubmissionData
{
    public sealed class GetSubmissionDataQuery : IRequest<Response>
    {
        public GetSubmissionDataQuery(int submissionId)
        {
            SubmissionId = submissionId;
        }

        public int SubmissionId { get; set; }


    }
}
