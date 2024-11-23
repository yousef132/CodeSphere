using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CodeSphere.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public new DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<UserContest> Registers { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemImage> ProblemImages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> TutorialImages { get; set; }
        public DbSet<Testcase> Testcases { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ProblemTopic> ProblemTopics { get; set; }
        public DbSet<Submit> Submits { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
