namespace CodeSphere.Domain.Models.Entities
{
    public class Topic
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<ProblemTopic> ProblemTopics { get; set; }
    }
}
