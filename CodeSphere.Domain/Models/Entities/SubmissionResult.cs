namespace CodeSphere.Domain.Models.Entities
{
    public enum SubmissionResult
    {
        Accepted,
        WrongAnswer,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        CompilationError,
        RunTimeError
    }
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
