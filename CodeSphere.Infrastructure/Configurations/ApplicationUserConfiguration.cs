using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeSphere.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.RankName)
                          .HasConversion(uStatus => uStatus.ToString(), OStatus => (Status)Enum.Parse(typeof(Status), OStatus));
        }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {



        }
    }

}
