using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Resume Head Pages (inherits Page)
    /// </summary>
    public class ResumeHeadPage : IEntityTypeConfiguration<DbModels.ResumeHeadPage>
    {
        public void Configure(EntityTypeBuilder<DbModels.ResumeHeadPage> builder)
        {
            //Index
            builder.HasKey(rhp => rhp.Id);
            builder.Property(rhp => rhp.Id)
                .IsRequired(true);

            //Other properties
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
