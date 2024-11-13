namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class RunTimeErrorResponse : BaseSubmissionResponse
    {
        public int TestCaseNumber { get; set; }
        public string Message { get; set; }
    }
}
