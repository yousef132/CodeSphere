using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Models.Entities
{
    public class BlogImage : BaseEntity
    {
        public Guid BlogId { get; set; }
        public string ImagePath { get; set; }
        public Blog Blog { get; set; }
    }
}
