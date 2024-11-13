using CodeSphere.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProblemTopicConfiguration : IEntityTypeConfiguration<ProblemTopic>
{
    public void Configure(EntityTypeBuilder<ProblemTopic> builder)
    {
        builder.HasKey(pt => new { pt.ProblemId, pt.TopicId });

        builder.HasOne(pt => pt.Problem)
            .WithMany(p => p.ProblemTopics)
            .HasForeignKey(pt => pt.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pt => pt.Topic)
            .WithMany(t => t.ProblemTopics)
            .HasForeignKey(pt => pt.TopicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
