using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Landing models
    /// </summary>
    public class Landing : IEntityTypeConfiguration<DbModels.Landing>
    {
        public void Configure(EntityTypeBuilder<DbModels.Landing> builder)
        {
            //Index
            builder.HasKey(l => l.Index);
            builder.Property(l => l.Index)
                .HasColumnType("INTEGER");

            //Other properties
            builder.Property(l => l.Id)
                .IsRequired(true);
            builder.HasIndex(l => l.Id)
                .IsUnique();
            builder.Property(l => l.Href)
                .HasMaxLength(Consts.MaxLinkLength)
                .IsRequired(false);
            builder.Property(l => l.Title)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(true);
            builder.Property(l => l.Subtitle)
                .HasMaxLength(Consts.MaxSubtitleLength)
                .IsRequired(false);
            builder.Property(l => l.Icon)
                .HasMaxLength(Consts.MaxTitleLength)
                .IsRequired(false);
            builder.Property(l => l.Order)
                .IsRequired(false);

            //Foreign keys
            builder
                .HasMany(l => l.Pages)
                .WithOne(p => p.Landing)
                .HasForeignKey(p => p.LandingId)
                .HasPrincipalKey(l => l.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
