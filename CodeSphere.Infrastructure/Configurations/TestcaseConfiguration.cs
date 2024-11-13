using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TestcaseConfiguration : IEntityTypeConfiguration<Testcase>
{
    public void Configure(EntityTypeBuilder<Testcase> builder)
    {
        builder.HasOne(tc => tc.Problem)
            .WithMany(p => p.Testcases)
            .HasForeignKey(tc => tc.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
