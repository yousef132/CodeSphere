using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class Contest : BaseEntity
    {
        public string ProblemSetterId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration => EndDate.Subtract(StartDate);
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int? BlogId { get; set; }

        public ContestStatus ContestStatus
            => DateTime.Now < StartDate ? ContestStatus.Upcoming : DateTime.Now > EndDate ? ContestStatus.Ended : ContestStatus.Running;

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }


        [ForeignKey(nameof(ProblemSetterId))]
        public ApplicationUser ProblemSetter { get; set; }

        public ICollection<UserContest> Registrations { get; set; }
        public ICollection<Problem> Problems { get; set; }
        public ICollection<Submit> Submissions { get; set; }
    }
}
