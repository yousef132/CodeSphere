namespace CodeSphere.Domain.CustomExceptions
{
    public class CodeExecutionException : Exception
    {
        public CodeExecutionException() { }

        public CodeExecutionException(string message)
            : base(message) { }
    }
}
