using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class Blog : BaseEntity
    {
        public string BlogCreatorId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public Contest Contest { get; set; }

        [ForeignKey(nameof(BlogCreatorId))]
        public ApplicationUser BlogCreator { get; set; }
        public ICollection<BlogImage> Images { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
