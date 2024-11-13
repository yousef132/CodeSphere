using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Responses
{
    public class TestCaseRunResult
    {
        public int TestCaseId { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public string? ActualOutput { get; set; }
        public SubmissionResult Result { get; set; }
        public decimal RunTime { get; set; }
        public decimal RunMemory { get; set; }
        public string Error { get; set; }
    }
}
