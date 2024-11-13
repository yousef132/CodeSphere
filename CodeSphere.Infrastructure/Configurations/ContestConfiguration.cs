using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ContestConfiguration : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.HasOne(c => c.ProblemSetter)
            .WithMany(u => u.Contests)
            .HasForeignKey(c => c.ProblemSetterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.Name)
            .IsRequired();
    }
}
