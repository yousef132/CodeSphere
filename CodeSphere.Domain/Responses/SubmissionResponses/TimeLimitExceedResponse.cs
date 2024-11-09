namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class TimeLimitExceedResponse : BaseSubmissionResponse
    {
        public int TestCaseNumber { get; set; }
        public decimal ExecutionTime { get; set; }
    }
}
