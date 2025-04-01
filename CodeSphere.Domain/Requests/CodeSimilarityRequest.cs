using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Requests
{
    public class CodeSimilarityRequest
    {
        public string Code1 { get; set; }
        public string Code2 { get; set; }
    }

    public class UserToCache
    {
        public string UserId { get; set; }
        public string ImagePath { get; set; }
        public string UserName { get; set; }

    }

    public class SubmissionToCache
    {
        public string UserId { get; set; }
        public int ProblemId { get; set; }
        public SubmissionResult Result { get; set; }
        public Language Language { get; set; }
        public DateTime Date { get; set; }
    }

}
