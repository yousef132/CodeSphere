namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public class CreateProblemCommandResponse
    {
        public int ContestId { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public decimal RunTimeLimit { get; set; }
        public decimal MemoryLimit { get; set; }
    }
}
