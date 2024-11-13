namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class MemoryLimitExceedResponse : BaseSubmissionResponse
    {
        public int TestCaseNumber { get; set; }
        public decimal ExecutionMemory { get; set; }
    }
}
