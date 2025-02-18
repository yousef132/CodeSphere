namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class RunTimeErrorResponse : BaseSubmissionResponse
    {
        public string Message { get; set; }

        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
    }
}
