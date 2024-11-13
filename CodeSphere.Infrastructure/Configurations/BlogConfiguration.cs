using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.HasOne(b => b.BlogCreator)
            .WithMany(u => u.Blogs)
            .HasForeignKey(b => b.BlogCreatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
