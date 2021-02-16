using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels
{

    /// <summary>
    /// Defines the Database Context that will provide the data storage for the application
    /// </summary>
    public class DatabaseContext : DbContext
    {
        private const int MaxTitleLength = 150;
        private const int MaxSubtitleLength = 350;
        private const int MaxLinkLength = 350;
        private const int MaxTextLength = 2048;

        /// <summary>
        /// Landings table, define overall landings sections of the application
        /// </summary>
        public DbSet<Landing> Landings { get; set; }
        /// <summary>
        /// Pages table, define individual pieces of landings to be viewed
        /// </summary>
        public DbSet<Page> Pages { get; set; }
        /// <summary>
        /// Image Wall Pages table, define extensions making select pages into walls of images
        /// </summary>
        public DbSet<ImageWallPage> ImageWallPages { get; set; }
        /// <summary>
        /// Resume Experience Jobs table, defines jobs to show on portfolio resume
        /// </summary>
        public DbSet<ResumeExpJob> ResumeExpJobs { get; set; }
        /// <summary>
        /// Resume Experience Pages table, defines extensions to select pages making them resume experience listings
        /// </summary>
        public DbSet<ResumeExpPage> ResumeExpPages { get; set; }
        /// <summary>
        /// Resume Head Pages table, defines extensions to select pages making them the head of resume landing sections
        /// </summary>
        public DbSet<ResumeHeadPage> ResumeHeadPages { get; set; }
        /// <summary>
        /// Schools table, defines schools to display having attended in the portfolio
        /// </summary>
        public DbSet<School> Schools { get; set; }
        /// <summary>
        /// Schools Pages table, defines extensions to select pages making them list schools attended
        /// </summary>
        public DbSet<SchoolsPage> SchoolsPages { get; set; }
        /// <summary>
        /// Text Block Pages table, defines extensions to select pages making them center around formatted Text Blocks
        /// </summary>
        public DbSet<TextBlockPage> TextBlockPages { get; set; }

        public DatabaseContext() : base() { }
        public DatabaseContext(DbContextOptions opts) : base(opts) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Below the code is defined for specifically outlining the database format
                //according to its needs.
                //Here, I favor exhaustively describing entities & relations over neglect

            builder.Entity<Landing>().HasKey(l => l.Id);
            builder.Entity<Landing>().Property(l => l.Id)
                .IsRequired(true);
            builder.Entity<Landing>().Property(l => l.Href)
                .HasMaxLength(MaxLinkLength)
                .IsRequired(false);
            builder.Entity<Landing>().Property(l => l.Title)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(true);
            builder.Entity<Landing>().Property(l => l.Subtitle)
                .HasMaxLength(MaxSubtitleLength)
                .IsRequired(false);
            builder.Entity<Landing>()
                .HasMany(l => l.Pages)
                .WithOne(p => p.Landing)
                .HasForeignKey(p => p.LandingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Page>().HasKey(p => p.Id);
            builder.Entity<Page>().Property(p => p.Id)
                .IsRequired(true);
            builder.Entity<Page>().Property(p => p.LandingId)
                .IsRequired(true);
            builder.Entity<Page>().Property(p => p.Order)
                .IsRequired(false);
            builder.Entity<Page>().Property(p => p.Type)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(true);
            builder.Entity<Page>().Property(p => p.Title)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<Page>().Property(p => p.Subtitle)
                .HasMaxLength(MaxSubtitleLength)
                .IsRequired(false);
            builder.Entity<Page>().Property(p => p.Background)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<Page>().Property(p => p.Image)
                .HasMaxLength(MaxLinkLength)
                .IsRequired(false);
            builder.Entity<Page>().Property(p => p.Orientation)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<Page>()
                .HasOne(p => p.ImageWallExt)
                .WithOne(iwp => iwp.Page)
                .HasForeignKey<ImageWallPage>(iwp => iwp.PageId);
            builder.Entity<Page>()
                .HasOne(p => p.ResumeExpExt)
                .WithOne(ree => ree.Page)
                .HasForeignKey<ResumeExpPage>(ree => ree.PageId);
            builder.Entity<Page>()
                .HasOne(p => p.ResumeHeadExt)
                .WithOne(rhe => rhe.Page)
                .HasForeignKey<ResumeHeadPage>(rhe => rhe.PageId);
            builder.Entity<Page>()
                .HasOne(p => p.SchoolsExt)
                .WithOne(se => se.Page)
                .HasForeignKey<SchoolsPage>(se => se.PageId);
            builder.Entity<Page>()
                .HasOne(p => p.TextBlockExt)
                .WithOne(tbe => tbe.Page)
                .HasForeignKey<TextBlockPage>(tbe => tbe.PageId);

            builder.Entity<ImageWallPage>().HasKey(p => p.Id);
            builder.Entity<ImageWallPage>().Property(p => p.Id)
                .IsRequired(true);
            builder.Entity<ImageWallPage>().Property(p => p.PageId)
                .IsRequired(true);
            builder.Entity<ImageWallPage>().Property(p => p.Description)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);
            builder.Entity<ImageWallPage>().Property(p => p.Images)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);

            builder.Entity<ResumeExpPage>().HasKey(rep => rep.Id);
            builder.Entity<ResumeExpPage>().Property(rep => rep.Id)
                .IsRequired(true);
            builder.Entity<ResumeExpPage>().Property(rep => rep.PageId)
                .IsRequired(true);
            builder.Entity<ResumeExpPage>()
                .HasMany(rep => rep.Jobs)
                .WithOne(rej => rej.Page)
                .HasForeignKey(rej => rej.PageId);

            builder.Entity<ResumeExpJob>().HasKey(rej => rej.Id);
            builder.Entity<ResumeExpJob>().Property(rej => rej.Id)
                .IsRequired(true);
            builder.Entity<ResumeExpJob>().Property(rej => rej.PageId)
                .IsRequired(true);
            builder.Entity<ResumeExpJob>().Property(rej => rej.Title)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(true);
            builder.Entity<ResumeExpJob>().Property(rej => rej.Employer)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<ResumeExpJob>().Property(rej => rej.EmployerCity)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<ResumeExpJob>().Property(rej => rej.StartDate)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<ResumeExpJob>().Property(rej => rej.EndDate)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<ResumeExpJob>().Property(rej => rej.Description)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);

            builder.Entity<ResumeHeadPage>().HasKey(rhp => rhp.Id);
            builder.Entity<ResumeHeadPage>().Property(rhp => rhp.Id)
                .IsRequired(true);
            builder.Entity<ResumeHeadPage>().Property(rhp => rhp.PageId)
                .IsRequired(true);
            builder.Entity<ResumeHeadPage>().Property(rhp => rhp.Description)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);
            builder.Entity<ResumeHeadPage>().Property(rhp => rhp.Competencies)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);

            builder.Entity<SchoolsPage>().HasKey(sp => sp.Id);
            builder.Entity<SchoolsPage>().Property(sp => sp.Id)
                .IsRequired(true);
            builder.Entity<SchoolsPage>().Property(sp => sp.PageId)
                .IsRequired(true);
            builder.Entity<SchoolsPage>()
                .HasMany(sp => sp.Schools)
                .WithOne(s => s.Page)
                .HasForeignKey(s => s.PageId);

            builder.Entity<School>().HasKey(s => s.Id);
            builder.Entity<School>().Property(s => s.Id)
                .IsRequired(true);
            builder.Entity<School>().Property(s => s.PageId)
                .IsRequired(true);
            builder.Entity<School>().Property(s => s.Name)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(true);
            builder.Entity<School>().Property(s => s.City)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<School>().Property(s => s.StartDate)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<School>().Property(s => s.EndDate)
                .HasMaxLength(MaxTitleLength)
                .IsRequired(false);
            builder.Entity<School>().Property(s => s.Program)
                .HasMaxLength(MaxSubtitleLength)
                .IsRequired(false);
            builder.Entity<School>().Property(s => s.GPA)
                .HasColumnType("decimal(8,6)")
                .IsRequired(false);
            builder.Entity<School>().Property(s => s.Other)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);

            builder.Entity<TextBlockPage>().HasKey(tbp => tbp.Id);
            builder.Entity<TextBlockPage>().Property(tbp => tbp.Id)
                .IsRequired(true);
            builder.Entity<TextBlockPage>().Property(tbp => tbp.PageId)
                .IsRequired(true);
            builder.Entity<TextBlockPage>().Property(tbp => tbp.Text)
                .HasMaxLength(MaxTextLength)
                .IsRequired(false);
        }
    }
}
