using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class BaseSubmissionResponse
    {
        //public decimal ExecutionMemory { get; set; }
        //public string Code { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public SubmissionResult SubmissionResult { get; set; } = SubmissionResult.Accepted;
        public int TotalTestcases { get; set; }
        public int NumberOfPassedTestCases { get; set; }
    }
}