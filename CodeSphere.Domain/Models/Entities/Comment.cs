using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSphere.Domain.Models.Entities
{
    public class Comment : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Path { get; set; }
        public string AuthorId { get; set; }
        public int BlogId { get; set; }

        public string Content { get; set; }
        public bool IsEdited { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }
    }
}
