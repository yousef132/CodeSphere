using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Models.Entities
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<ProblemTopic> ProblemTopics { get; set; }
    }
}
