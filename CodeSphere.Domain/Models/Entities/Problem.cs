using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class Problem : BaseEntity
    {
        public string ProblemSetterId { get; set; }
        public int ContestId { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }

        public decimal RunTimeLimit { get; set; }
        public decimal MemoryLimit { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(ProblemSetterId))]
        public ApplicationUser ProblemSetter { get; set; }

        [ForeignKey(nameof(ContestId))]
        public Contest Contest { get; set; }
        public ICollection<ProblemImage> Images { get; set; }

        public ICollection<Testcase> Testcases { get; set; }

        public ICollection<ProblemTopic> ProblemTopics { get; set; }

        public ICollection<Submit> Submissions { get; set; }
    }
}
