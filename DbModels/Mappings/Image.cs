using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    public class Image : IEntityTypeConfiguration<DbModels.Image>
    {
        public void Configure(EntityTypeBuilder<DbModels.Image> builder)
        {
            //Index
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .IsRequired(true);

            //Other properties
            builder.Property(i => i.PageId)
                .IsRequired(true);
            builder.Property(i => i.Source)
                .HasMaxLength(Consts.MaxLinkLength)
                .IsRequired(true);
            builder.Property(i => i.Description)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);
            builder.Property(i => i.Order)
                .HasColumnType("integer")
                .IsRequired(false);
        }
    }
}
