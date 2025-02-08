namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class TimeLimitExceedResponse : BaseSubmissionResponse
    {
        public decimal ExecutionTime { get; set ; }
    }
}
