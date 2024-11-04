using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class Submit : BaseEntity
    {
        public string UserId { get; set; }
        public int ProblemId { get; set; }
        public int? ContestId { get; set; }
        public string? Error { get; set; }
        // the code execution time
        public decimal SubmitTime { get; set; }
        // the code execution memory
        public decimal SubmitMemory { get; set; }
        public SubmissionResult Result { get; set; }
        public string Code { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public Language Language { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(ProblemId))]
        public Problem Problem { get; set; }

        [ForeignKey(nameof(ContestId))]
        public Contest Contest { get; set; }
    }
}
