namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class TimeLimitExceedResponse : BaseSubmissionResponse
    {
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
    }
}
