using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Models
{
    public class Problem : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float Rate { get; set; }

        // ans so on....
    }
}
