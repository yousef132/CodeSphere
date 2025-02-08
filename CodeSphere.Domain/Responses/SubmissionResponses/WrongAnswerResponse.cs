namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class WrongAnswerResponse : BaseSubmissionResponse
    {
        public string ExpectedOutput { get; set; }
        public string ActualOutput { get; set; }
    }
}
