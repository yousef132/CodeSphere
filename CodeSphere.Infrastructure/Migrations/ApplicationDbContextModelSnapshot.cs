﻿// <auto-generated />
using System;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodeSphere.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BlogCreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BlogCreatorId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.BlogImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("TutorialImages");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("bit");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BlogId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Contest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BlogId")
                        .HasColumnType("int");

                    b.Property<decimal>("Duration")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProblemSetterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BlogId")
                        .IsUnique()
                        .HasFilter("[BlogId] IS NOT NULL");

                    b.HasIndex("ProblemSetterId");

                    b.ToTable("Contests");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContestId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<decimal>("MemoryLimit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProblemSetterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("RunTimeLimit")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ContestId");

                    b.HasIndex("ProblemSetterId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.ProblemImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProblemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("ProblemImages");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.ProblemTopic", b =>
                {
                    b.Property<int>("ProblemId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("ProblemId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("ProblemTopics");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Submit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ContestId")
                        .HasColumnType("int");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Language")
                        .HasColumnType("int");

                    b.Property<int>("ProblemId")
                        .HasColumnType("int");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("SubmitMemory")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SubmitTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ContestId");

                    b.HasIndex("ProblemId");

                    b.HasIndex("UserId");

                    b.ToTable("Submits");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Testcase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProblemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("Testcases");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.UserContest", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ContestId")
                        .HasColumnType("int");

                    b.Property<short>("RankChange")
                        .HasColumnType("smallint");

                    b.HasKey("UserId", "ContestId");

                    b.HasIndex("ContestId");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Identity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Rating")
                        .HasColumnType("smallint");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Blog", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", "BlogCreator")
                        .WithMany("Blogs")
                        .HasForeignKey("BlogCreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BlogCreator");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.BlogImage", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Blog", "Blog")
                        .WithMany("Images")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Comment", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeSphere.Domain.Models.Entities.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Contest", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Blog", "Blog")
                        .WithOne("Contest")
                        .HasForeignKey("CodeSphere.Domain.Models.Entities.Contest", "BlogId");

                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", "ProblemSetter")
                        .WithMany("Contests")
                        .HasForeignKey("ProblemSetterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Blog");

                    b.Navigation("ProblemSetter");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Problem", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Contest", "Contest")
                        .WithMany("Problems")
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", "ProblemSetter")
                        .WithMany("Problems")
                        .HasForeignKey("ProblemSetterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Contest");

                    b.Navigation("ProblemSetter");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.ProblemImage", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Problem", "Problem")
                        .WithMany("Images")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.ProblemTopic", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Problem", "Problem")
                        .WithMany("ProblemTopics")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeSphere.Domain.Models.Entities.Topic", "Topic")
                        .WithMany("ProblemTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Submit", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Contest", "Contest")
                        .WithMany("Submissions")
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeSphere.Domain.Models.Entities.Problem", "Problem")
                        .WithMany("Submissions")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", "User")
                        .WithMany("Submissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contest");

                    b.Navigation("Problem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Testcase", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Problem", "Problem")
                        .WithMany("Testcases")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.UserContest", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Entities.Contest", "Contest")
                        .WithMany("Registrations")
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", "User")
                        .WithMany("Registrations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CodeSphere.Domain.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Blog", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Contest")
                        .IsRequired();

                    b.Navigation("Images");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Contest", b =>
                {
                    b.Navigation("Problems");

                    b.Navigation("Registrations");

                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Problem", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("ProblemTopics");

                    b.Navigation("Submissions");

                    b.Navigation("Testcases");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Entities.Topic", b =>
                {
                    b.Navigation("ProblemTopics");
                });

            modelBuilder.Entity("CodeSphere.Domain.Models.Identity.ApplicationUser", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Comments");

                    b.Navigation("Contests");

                    b.Navigation("Problems");

                    b.Navigation("Registrations");

                    b.Navigation("Submissions");
                });
#pragma warning restore 612, 618
        }
    }
}
