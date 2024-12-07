using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Features.Submission.Queries.GetSubmissionData
{
    public class GetSubmissionDataQueryResponse
    {
        public string Code { get; set; }
        public decimal SubmitTime { get; set; }
        public decimal SubmitMemory { get; set; }
        public Language Language { get; set; }
        public SubmissionResult Result { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? Error { get; set; }
    }
}
