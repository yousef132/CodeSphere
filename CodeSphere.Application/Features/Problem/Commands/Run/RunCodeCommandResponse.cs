namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommandResponse
    {
        public RunCodeCommandResponse(string input, string expectedOutput, bool passed)
        {
            Input = input;
            this.expectedOutput = expectedOutput;
            Passed = passed;
        }

        public string Input { get; set; }
        public string expectedOutput { get; set; }
        public bool Passed { get; set; }

    }
}
