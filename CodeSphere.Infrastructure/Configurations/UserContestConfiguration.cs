using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserContestConfiguration : IEntityTypeConfiguration<UserContest>
{
    public void Configure(EntityTypeBuilder<UserContest> builder)
    {
        builder.HasKey(r => new { r.UserId, r.ContestId });

        builder.HasOne(r => r.User)
            .WithMany(u => u.Registrations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Contest)
            .WithMany(c => c.Registrations)
            .HasForeignKey(r => r.ContestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

