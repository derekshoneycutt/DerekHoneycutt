using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Resume Experience Job models.
    /// </summary>
    public class ResumeExpJob : IEntityTypeConfiguration<DbModels.ResumeExpJob>
    {
        public void Configure(EntityTypeBuilder<DbModels.ResumeExpJob> builder)
        {
            //Index
            builder.HasKey(rej => rej.Id);
            builder.Property(rej => rej.Id)
                .IsRequired(true);

            //Other properties
            builder.Property(rej => rej.PageId)
                .IsRequired(true);
            builder.Property(rej => rej.Title)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(true);
            builder.Property(rej => rej.Employer)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(rej => rej.EmployerCity)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(rej => rej.StartDate)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(rej => rej.EndDate)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(rej => rej.Description)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);
        }
    }
}
