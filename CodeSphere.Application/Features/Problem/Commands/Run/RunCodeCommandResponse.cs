namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommandResponse
    {
        public RunCodeCommandResponse(string input, string output, bool passed)
        {
            Input = input;
            Output = output;
            Passed = passed;
        }

        public string Input { get; set; }
        public string Output { get; set; }
        public bool Passed { get; set; }

    }
}
