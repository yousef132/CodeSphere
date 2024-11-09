namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class WrongAnswerResponse : BaseSubmissionResponse
    {
        public int TestCaseNumber { get; set; }
        public string ExpectedOutput { get; set; }
        public string ActualOutput { get; set; }
    }
}
