using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Infrastructure.Configurations
{
	internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(u => u.Status)
						  .HasConversion(uStatus => uStatus.ToString(), OStatus => (Status)Enum.Parse(typeof(Status), OStatus));
		}
	}
}
