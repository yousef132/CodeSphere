using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Responses.ElasticSearchResponses
{
    public class ProblemDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public List<int> Topics { get; set; }
    }
}
