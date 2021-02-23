using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Image Wall Pages (inherits Page)
    /// </summary>
    public class ImageWallPage : IEntityTypeConfiguration<DbModels.ImageWallPage>
    {
        public void Configure(EntityTypeBuilder<DbModels.ImageWallPage> builder)
        {
            //Index
            builder.HasKey(p => p.Index);
            builder.Property(p => p.Index)
                .HasColumnType("INTEGER");

            //Other properties
            builder.Property(p => p.Id)
                .IsRequired(true);
            builder.HasIndex(p => p.Id)
                .IsUnique();
            builder.Property(p => p.PageId)
                .IsRequired(true);
            builder.Property(p => p.Description)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);

            //Foreign keys
            builder
                .HasMany(p => p.Images)
                .WithOne(i => i.Page)
                .HasForeignKey(i => i.PageId)
                .HasPrincipalKey(iwp => iwp.Id);
        }
    }
}
