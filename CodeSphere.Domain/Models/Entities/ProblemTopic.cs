using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class ProblemTopic
    {
        public Guid ProblemId { get; set; }
        public Guid TopicId { get; set; }

        [ForeignKey(nameof(ProblemId))]
        public Problem Problem { get; set; }

        [ForeignKey(nameof(TopicId))]
        public Topic Topic { get; set; }
    }
}
