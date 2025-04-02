namespace CodeSphere.Domain.Responses.Contest
{
    public class StandingDto : ContestStandingResposne
    {
        // for problemId = 1, the submissionId = 2 and the date = ~ and the language = C# and the fail count = 0
        public List<UserProblemSubmissionWithoutUserId>? UserProblemSubmissions { get; set; }
    }
    public class ContestStandingResposne
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? ImagePath { get; set; }
        public int TotalPoints { get; set; }
    }

    public class UserProblemSubmission : UserProblemSubmissionWithoutUserId
    {
        public string UserId { get; set; }

    }
    public class UserProblemSubmissionWithoutUserId : ProblemSubmissionsCount
    {
        public int ProblemId { get; set; }
    }
    public class ProblemSubmissionsCount
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public DateTime? EarliestSuccessDate { get; set; }

    }

}
