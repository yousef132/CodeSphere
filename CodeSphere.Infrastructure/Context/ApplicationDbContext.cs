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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            // Composite keys for many-to-many relationships
            ConfigureCompositeKeys(modelBuilder);

            // Configure relationships
            ConfigureRelationships(modelBuilder);
        }
        private static void ConfigureCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserContest>()
                .HasKey(r => new { r.UserId, r.ContestId });

            modelBuilder.Entity<ProblemTopic>()
                .HasKey(pt => new { pt.ProblemId, pt.TopicId });
        }

        private static void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // One-to-Many: User -> Contest (Setter)
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.ProblemSetter)
                .WithMany(u => u.Contests)
                .HasForeignKey(c => c.ProblemSetterId)
                .OnDelete(DeleteBehavior.Restrict);



            // One-to-Many: Problem -> Testcase
            modelBuilder.Entity<Testcase>()
                .HasOne(tc => tc.Problem)
                .WithMany(p => p.Testcases)
                .HasForeignKey(tc => tc.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> Blog (Setter)
            modelBuilder.Entity<Blog>()
                .HasOne(t => t.BlogCreator)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.BlogCreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Blog -> BlogImage
            modelBuilder.Entity<BlogImage>()
                .HasOne(ti => ti.Blog)
                .WithMany(t => t.Images)
                .HasForeignKey(ti => ti.BlogId)
                .OnDelete(DeleteBehavior.Cascade);



            // One-to-Many: User -> Register
            modelBuilder.Entity<UserContest>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Contest -> Register
            modelBuilder.Entity<UserContest>()
                .HasOne(r => r.Contest)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> Submit
            modelBuilder.Entity<Submit>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Problem -> Submit
            modelBuilder.Entity<Submit>()
                .HasOne(s => s.Problem)
                .WithMany(p => p.Submissions)
                .HasForeignKey(s => s.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Contest -> Submit
            modelBuilder.Entity<Submit>()
                .HasOne(s => s.Contest)
                .WithMany(c => c.Submissions)
                .HasForeignKey(s => s.ContestId)
                .OnDelete(DeleteBehavior.Cascade);



            //=======================ss
            modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Email)
            .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Contest>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Problem>()
                .Property(p => p.Name)
                .IsRequired();
        }
    }
}
