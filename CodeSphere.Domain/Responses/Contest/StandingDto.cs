using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Responses.Contest
{
    public class StandingDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserImage { get; set; }
        public decimal Rank { get; set; }

        // for problemId = 1, the submissionId = 2 and the date = ~ and the language = C# and the fail count = 0
        public List<Dictionary<int, UserProblemSubmission>> UserProblemSubmissions { get; set; }
    }

    public class UserProblemSubmission
    {
        public int SubmissionId { get; set; }
        public DateTime SubmissionDate { get; set; }

        public Language Language { get; set; }
        public int FailCount { get; set; }
    }

}
