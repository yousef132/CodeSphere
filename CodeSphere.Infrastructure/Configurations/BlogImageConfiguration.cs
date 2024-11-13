using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BlogImageConfiguration : IEntityTypeConfiguration<BlogImage>
{
    public void Configure(EntityTypeBuilder<BlogImage> builder)
    {
        builder.HasOne(ti => ti.Blog)
            .WithMany(t => t.Images)
            .HasForeignKey(ti => ti.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
