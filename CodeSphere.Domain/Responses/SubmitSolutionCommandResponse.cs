using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Responses
{
    public class SubmitSolutionCommandResponse
    {
        public int ProblemId { get; set; }
        public List<TestCaseRunResult> TestCaseRuns { get; set; }
        public decimal SubmitTime { get; set; }
        public SubmissionResult SubmissionResult { get; set; }

        public string? Error { get; set; }
    }
}
