using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Identity;

namespace CodeSphere.Domain.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserStatus RankName { get; set; } = UserStatus.UnRanked; // ex: pupil
        public string? ImagePath { get; set; }
        public short Rating { get; set; } = 0; // ex: 1200 
        public Gender? Gender { get; set; }

        public string? Name { get; set; }

        public ICollection<Contest> Contests { get; set; } // Contests created by this user (Setter)
        public ICollection<Problem> Problems { get; set; } // Problems set by this user (Setter)
        public ICollection<Blog> Blogs { get; set; } // Tutorials created by this user (Setter)
        public ICollection<UserContest> Registrations { get; set; } // Contests registered by this user
        public ICollection<Submit> Submissions { get; set; }
        public ICollection<Comment> Comments { get; set; }


    }
}
