using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeSphere.Infrastructure.Configurations
{
    public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
    {
        public void Configure(EntityTypeBuilder<Problem> builder)
        {
            // One-to-Many: User -> Problem (Setter)
            builder.HasOne(p => p.ProblemSetter)
                   .WithMany(u => u.Problems)
                   .HasForeignKey(p => p.ProblemSetterId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Contest -> Problem

            builder.HasOne(p => p.Contest)
                   .WithMany(c => c.Problems)
                   .HasForeignKey(p => p.ContestId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Problem -> ProblemImage
            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Problem)
                .HasForeignKey(pi => pi.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
