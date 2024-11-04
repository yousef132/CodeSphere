using CodeSphere.Domain.Premitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class Testcase : BaseEntity
    {
        public int ProblemId { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

        [ForeignKey(nameof(ProblemId))]
        [InverseProperty(nameof(Problem.Testcases))]
        public Problem Problem { get; set; }
    }
}
