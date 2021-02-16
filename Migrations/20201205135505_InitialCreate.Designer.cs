﻿// <auto-generated />
using System;
using DerekHoneycutt.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DerekHoneycutt.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20201205135505_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DerekHoneycutt.DbModels.ImageWallPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("Images")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("ImageWallPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Landing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Href")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<string>("Subtitle")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Landings");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Page", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Background")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LandingId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Orientation")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Subtitle")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LandingId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("Employer")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployerCity")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("EndDate")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartDate")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId");

                    b.ToTable("ResumeExpJobs");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("ResumeExpPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeHeadPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Competencies")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("ResumeHeadPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.School", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("EndDate")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("GPA")
                        .HasColumnType("decimal(8,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Other")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Program")
                        .HasMaxLength(350)
                        .HasColumnType("TEXT");

                    b.Property<string>("StartDate")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.SchoolsPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("SchoolsPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.TextBlockPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PageId")
                        .IsUnique();

                    b.ToTable("TextBlockPages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ImageWallPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("ImageWallExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.ImageWallPage", "PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Page", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Landing", "Landing")
                        .WithMany("Pages")
                        .HasForeignKey("LandingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Landing");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpJob", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.ResumeExpPage", "Page")
                        .WithMany("Jobs")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeExpPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("ResumeExpExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.ResumeExpPage", "PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.ResumeHeadPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("ResumeHeadExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.ResumeHeadPage", "PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.School", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.SchoolsPage", "Page")
                        .WithMany("Schools")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.SchoolsPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("SchoolExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.SchoolsPage", "PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.TextBlockPage", b =>
                {
                    b.HasOne("DerekHoneycutt.DbModels.Page", "Page")
                        .WithOne("TextBlockExt")
                        .HasForeignKey("DerekHoneycutt.DbModels.TextBlockPage", "PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Landing", b =>
                {
                    b.Navigation("Pages");
                });

            modelBuilder.Entity("DerekHoneycutt.DbModels.Page", b =>
                {
                    b.Navigation("ImageWallExt");

                    b.Navigation("ResumeExpExt");

                    b.Navigation("ResumeHeadExt");

                    b.Navigation("SchoolExt");

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
