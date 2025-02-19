using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Responses
{
    public class ContestProblemResponse
    {
        public string ProblemSetterId { get; set; }
        public int ContestId { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }

        public decimal RunTimeLimit { get; set; }
        public MemoryLimit MemoryLimit { get; set; }

        public string Description { get; set; }
        public ContestPoints ContestPoints { get; set; }
    }
}
