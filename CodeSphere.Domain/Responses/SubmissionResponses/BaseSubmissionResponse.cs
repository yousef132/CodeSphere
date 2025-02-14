using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class BaseSubmissionResponse
    {
        public decimal ExecutionTime { get; set; }
        public decimal ExecutionMemory { get; set; }
        //public string Code { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public SubmissionResult SubmissionResult { get; set; } = SubmissionResult.Accepted;
        public int NumberOfPassedTestCases { get; set; }
        public string Input { get; set; } = string.Empty;
    }
}