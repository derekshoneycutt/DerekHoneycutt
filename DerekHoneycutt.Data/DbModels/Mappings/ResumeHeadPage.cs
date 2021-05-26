using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Resume Head Pages (inherits Page)
    /// </summary>
    public class ResumeHeadPage : IEntityTypeConfiguration<DbModels.ResumeHeadPage>
    {
        public void Configure(EntityTypeBuilder<DbModels.ResumeHeadPage> builder)
        {
            //Index
            builder.HasKey(rhp => rhp.Index);
            builder.Property(rhp => rhp.Index)
                .HasColumnType("INTEGER");

            //Other properties
            builder.Property(rhp => rhp.Id)
                .IsRequired(true);
            builder.HasIndex(rhp => rhp.Id)
                .IsUnique();
            builder.Property(rhp => rhp.PageId)
                .IsRequired(true);
            builder.Property(rhp => rhp.Description)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);
            builder.Property(rhp => rhp.Competencies)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);
        }
    }
}
