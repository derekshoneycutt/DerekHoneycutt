using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Schools Pages (inherits Page)
    /// </summary>
    public class SchoolsPage : IEntityTypeConfiguration<DbModels.SchoolsPage>
    {
        public void Configure(EntityTypeBuilder<DbModels.SchoolsPage> builder)
        {
            //Index
            builder.HasKey(sp => sp.Index);
            builder.Property(sp => sp.Index)
                .HasColumnType("INTEGER");

            //Other properties
            builder.Property(sp => sp.Id)
                .IsRequired(true);
            builder.HasIndex(sp => sp.Id)
                .IsUnique();
            builder.Property(sp => sp.PageId)
                .IsRequired(true);

            //Foreign keys
            builder
                .HasMany(sp => sp.Schools)
                .WithOne(s => s.Page)
                .HasForeignKey(s => s.PageId)
                .HasPrincipalKey(sp => sp.Id);
        }
    }
}
