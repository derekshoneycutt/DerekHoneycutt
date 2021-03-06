﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Resume Experience Pages (inherits Page)
    /// </summary>
    public class ResumeExpPage : IEntityTypeConfiguration<DbModels.ResumeExpPage>
    {
        public void Configure(EntityTypeBuilder<DbModels.ResumeExpPage> builder)
        {
            //Index
            builder.HasKey(rep => rep.Index);
            builder.Property(rep => rep.Index)
                .HasColumnType("INTEGER");

            //Other properties
            builder.Property(rep => rep.Id)
                .IsRequired(true);
            builder.HasIndex(rep => rep.Id)
                .IsUnique();
            builder.Property(rep => rep.PageId)
                .IsRequired(true);

            //Foreign Keys
            builder
                .HasMany(rep => rep.Jobs)
                .WithOne(rej => rej.Page)
                .HasForeignKey(rej => rej.PageId)
                .HasPrincipalKey(rep => rep.Id);
        }
    }
}
