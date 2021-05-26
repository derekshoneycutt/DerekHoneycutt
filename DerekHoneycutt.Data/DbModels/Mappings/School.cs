using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for School models.
    /// </summary>
    public class School : IEntityTypeConfiguration<DbModels.School>
    {
        public void Configure(EntityTypeBuilder<DbModels.School> builder)
        {
            //Index
            builder.HasKey(s => s.Index);
            builder.Property(s => s.Index)
                .HasColumnType("INTEGER");

            //Other properties
            builder.Property(s => s.Id)
                .IsRequired(true);
            builder.HasIndex(s => s.Id)
                .IsUnique();
            builder.Property(s => s.PageId)
                .IsRequired(true);
            builder.Property(s => s.Name)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(true);
            builder.Property(s => s.City)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(s => s.StartDate)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(s => s.EndDate)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(s => s.Program)
                .HasMaxLength(Consts.MaxSubtitleLength)
                .IsRequired(false);
            builder.Property(s => s.GPA)
                .HasColumnType("decimal(8,6)")
                .IsRequired(false);
            builder.Property(s => s.Other)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);
            builder.Property(s => s.Order)
                .HasColumnType("integer")
                .IsRequired(false);
        }
    }
}
