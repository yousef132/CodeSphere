using CodeSphere.Domain.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class UserContest
    {
        public string UserId { get; set; }
        public int ContestId { get; set; }


        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(ContestId))]
        public Contest Contest { get; set; }

        // the increase || decrease of rank
        public short RankChange { get; set; } = 0;
    }
}
