﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbModels.Mappings
{
    /// <summary>
    /// Entity configuration for Text Block Pages (inherits Page)
    /// </summary>
    public class TextBlockPage : IEntityTypeConfiguration<DbModels.TextBlockPage>
    {
        public void Configure(EntityTypeBuilder<DbModels.TextBlockPage> builder)
        {
            //Index
            builder.HasKey(tbp => tbp.Id);
            builder.Property(tbp => tbp.Id)
                .IsRequired(true);

            //Other properties
            builder.Property(tbp => tbp.PageId)
                .IsRequired(true);
            builder.Property(tbp => tbp.Text)
                .HasMaxLength(Consts.MaxTextLength)
                .IsRequired(false);
        }
    }
}
