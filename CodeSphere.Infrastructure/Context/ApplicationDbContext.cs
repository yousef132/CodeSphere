using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CodeSphere.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public new DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemImage> ProblemImages { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<TutorialImage> TutorialImages { get; set; }
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
            modelBuilder.Entity<Register>()
                .HasKey(r => new { r.UID, r.CID });

            modelBuilder.Entity<ProblemTopic>()
                .HasKey(pt => new { pt.PID, pt.TID });
        }

        private static void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // One-to-Many: User -> Contest (Setter)
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.Setter)
                .WithMany(u => u.Contests)
                .HasForeignKey(c => c.SetterID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: User -> Problem (Setter)
            modelBuilder.Entity<Problem>()
                .HasOne(p => p.Setter)
                .WithMany(u => u.Problems)
                .HasForeignKey(p => p.SetterID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Contest -> Problem
            modelBuilder.Entity<Problem>()
                .HasOne(p => p.Contest)
                .WithMany(c => c.Problems)
                .HasForeignKey(p => p.CID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Problem -> ProblemImage
            modelBuilder.Entity<ProblemImage>()
                .HasOne(pi => pi.Problem)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Problem -> Testcase
            modelBuilder.Entity<Testcase>()
                .HasOne(tc => tc.Problem)
                .WithMany(p => p.Testcases)
                .HasForeignKey(tc => tc.PID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> Tutorial (Setter)
            modelBuilder.Entity<Tutorial>()
                .HasOne(t => t.Setter)
                .WithMany(u => u.Tutorials)
                .HasForeignKey(t => t.SetterID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Problem -> Tutorial
            modelBuilder.Entity<Tutorial>()
                .HasOne(t => t.Problem)
                .WithMany(p => p.Tutorials)
                .HasForeignKey(t => t.PID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Tutorial -> TutorialImage
            modelBuilder.Entity<TutorialImage>()
                .HasOne(ti => ti.Tutorial)
                .WithMany(t => t.Images)
                .HasForeignKey(ti => ti.TID)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many: Problem <-> Topic (via ProblemTopic)
            modelBuilder.Entity<ProblemTopic>()
                .HasOne(pt => pt.Problem)
                .WithMany(p => p.ProblemTopics)
                .HasForeignKey(pt => pt.PID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProblemTopic>()
                .HasOne(pt => pt.Topic)
                .WithMany(t => t.ProblemTopics)
                .HasForeignKey(pt => pt.TID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> Register
            modelBuilder.Entity<Register>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Contest -> Register
            modelBuilder.Entity<Register>()
                .HasOne(r => r.Contest)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.CID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> Submit
            modelBuilder.Entity<Submit>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.UID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Problem -> Submit
            modelBuilder.Entity<Submit>()
                .HasOne(s => s.Problem)
                .WithMany(p => p.Submissions)
                .HasForeignKey(s => s.PID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Contest -> Submit
            modelBuilder.Entity<Submit>()
                .HasOne(s => s.Contest)
                .WithMany(c => c.Submissions)
                .HasForeignKey(s => s.CID)
                .OnDelete(DeleteBehavior.Cascade);



            //=======================ss
            modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

            modelBuilder.Entity<User>()
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
