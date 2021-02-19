using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Page models
    /// </summary>
    public class Page : IEntityTypeConfiguration<DbModels.Page>
    {
        public void Configure(EntityTypeBuilder<DbModels.Page> builder)
        {
            //Index
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired(true);

            //Other properties
            builder.Property(p => p.LandingId)
                .IsRequired(true);
            builder.Property(p => p.Order)
                .IsRequired(false);
            builder.Property(p => p.Type)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(true);
            builder.Property(p => p.Title)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(p => p.Subtitle)
                .HasMaxLength(Consts.MaxSubtitleLength)
                .IsRequired(false);
            builder.Property(p => p.Background)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(p => p.Image)
                .HasMaxLength(Consts.MaxLinkLength)
                .IsRequired(false);
            builder.Property(p => p.Orientation)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);

            //Foreign keys
                // The following all create an inheritance type connection, defined here
            builder
                .HasOne(p => p.ImageWallExt)
                .WithOne(iwp => iwp.Page)
                .HasForeignKey<DbModels.ImageWallPage>(iwp => iwp.PageId);
            builder
                .HasOne(p => p.ResumeExpExt)
                .WithOne(ree => ree.Page)
                .HasForeignKey<DbModels.ResumeExpPage>(ree => ree.PageId);
            builder
                .HasOne(p => p.ResumeHeadExt)
                .WithOne(rhe => rhe.Page)
                .HasForeignKey<DbModels.ResumeHeadPage>(rhe => rhe.PageId);
            builder
                .HasOne(p => p.GitHubPageExt)
                .WithOne(rhe => rhe.Page)
                .HasForeignKey<DbModels.GitHubPage>(rhe => rhe.PageId);
            builder
                .HasOne(p => p.SchoolsExt)
                .WithOne(se => se.Page)
                .HasForeignKey<DbModels.SchoolsPage>(se => se.PageId);
            builder
                .HasOne(p => p.TextBlockExt)
                .WithOne(tbe => tbe.Page)
                .HasForeignKey<DbModels.TextBlockPage>(tbe => tbe.PageId);
        }
    }
}
