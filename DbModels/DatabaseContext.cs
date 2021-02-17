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
            builder.ApplyConfiguration(new Mappings.Landing())
                .ApplyConfiguration(new Mappings.Page())
                .ApplyConfiguration(new Mappings.ImageWallPage())
                .ApplyConfiguration(new Mappings.ResumeExpPage())
                .ApplyConfiguration(new Mappings.ResumeExpJob())
                .ApplyConfiguration(new Mappings.ResumeHeadPage())
                .ApplyConfiguration(new Mappings.SchoolsPage())
                .ApplyConfiguration(new Mappings.School())
                .ApplyConfiguration(new Mappings.TextBlockPage());

            base.OnModelCreating(builder);
        }
    }
}
