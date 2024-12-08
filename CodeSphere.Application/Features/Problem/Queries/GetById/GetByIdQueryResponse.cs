using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
    public class GetByIdQueryResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public List<TestCasesDto> TestCases { get; set; }
        public List<TopicDto> Topics { get; set; }

        public decimal Accepted { get; set; }
        public decimal Submissions { get; set; }
        public decimal AcceptanceRate => Submissions > 0 ? Accepted / Submissions * 100 : 0;

        public bool IsSolved { get; set; }

    }
}
