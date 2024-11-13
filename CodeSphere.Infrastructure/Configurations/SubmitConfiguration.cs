using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SubmitConfiguration : IEntityTypeConfiguration<Submit>
{
    public void Configure(EntityTypeBuilder<Submit> builder)
    {
        builder.HasOne(s => s.User)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Problem)
            .WithMany(p => p.Submissions)
            .HasForeignKey(s => s.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Contest)
            .WithMany(c => c.Submissions)
            .HasForeignKey(s => s.ContestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
