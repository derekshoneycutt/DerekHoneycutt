﻿// <auto-generated />
using System;
using DerekHoneycutt.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DerekHoneycutt.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210223175907_IncreaseMaxExtraTextLength")]
    partial class IncreaseMaxExtraTextLength
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DerekHoneycutt.DbModels.GitHubPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GitHub")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Index")
                        .HasColumnType("int");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("GitHubPage");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Image", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Order")
                        .HasColumnType("integer");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ImageWallPage", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("ImageWallPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Landing", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Href")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("Icon")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Subtitle")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Landings");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Page", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Background")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<Guid>("LandingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Orientation")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Subtitle")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("Title")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("LandingId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpJob", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employer")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("EmployerCity")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("EndDate")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StartDate")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId");

                    b.ToTable("ResumeExpJobs");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpPage", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("ResumeExpPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeHeadPage", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Competencies")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("ResumeHeadPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.School", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("EndDate")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<decimal?>("GPA")
                        .HasColumnType("decimal(8,6)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Other")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Program")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("StartDate")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.SchoolsPage", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("SchoolsPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.TextBlockPage", b =>
                {
                    b.Property<int?>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Index");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("TextBlockPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.GitHubPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("GitHubPageExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.GitHubPage", "PageId")
                        .HasPrincipalKey("DerekHoneycutt.DbModels.Page", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Image", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.ImageWallPage", "Page")
                        .WithMany("Images")
                        .HasForeignKey("PageId")
                        .HasPrincipalKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ImageWallPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("ImageWallExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.ImageWallPage", "PageId")
                        .HasPrincipalKey("DerekHoneycutt.DbModels.Page", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Page", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Landing", "Landing")
                        .WithMany("Pages")
                        .HasForeignKey("LandingId")
                        .HasPrincipalKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Landing");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpJob", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.ResumeExpPage", "Page")
                        .WithMany("Jobs")
                        .HasForeignKey("PageId")
                        .HasPrincipalKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("ResumeExpExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.ResumeExpPage", "PageId")
                        .HasPrincipalKey("DerekHoneycutt.DbModels.Page", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeHeadPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("ResumeHeadExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.ResumeHeadPage", "PageId")
                        .HasPrincipalKey("DerekHoneycutt.DbModels.Page", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.School", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.SchoolsPage", "Page")
                        .WithMany("Schools")
                        .HasForeignKey("PageId")
                        .HasPrincipalKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.SchoolsPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("SchoolsExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.SchoolsPage", "PageId")
                        .HasPrincipalKey("DerekHoneycutt.DbModels.Page", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.TextBlockPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("TextBlockExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.TextBlockPage", "PageId")
                        .HasPrincipalKey("DerekHoneycutt.DbModels.Page", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ImageWallPage", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Landing", b =>
                {
                    b.Navigation("Pages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Page", b =>
                {
                    b.Navigation("GitHubPageExt");

                    b.Navigation("ImageWallExt");

                    b.Navigation("ResumeExpExt");

                    b.Navigation("ResumeHeadExt");

                    b.Navigation("SchoolsExt");

                    b.Navigation("TextBlockExt");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpPage", b =>
                {
                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.SchoolsPage", b =>
                {
                    b.Navigation("Schools");
                });
#pragma warning restore 612, 618
        }
    }
}
