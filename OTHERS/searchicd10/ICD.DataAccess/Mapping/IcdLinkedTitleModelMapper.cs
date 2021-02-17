using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess.Mapping
{
    /// <summary>
    /// Class used to map the IcdLinkedTitle object to a SQL Database
    /// </summary>
    public class IcdLinkedTitleModelMapper : EntityTypeConfiguration<IcdLinkedTitle>
    {
        public IcdLinkedTitleModelMapper()
        {
            //Table
            this.ToTable(ConstData.Table_LinkedTitles);

            //Primary Key
            this.HasKey(l => l.Id)
                .Property(l => l.Id)
                .HasColumnName(ConstData.Table_LinkedTitles_Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            //Other Columns
            this.Property(l => l.Code).HasColumnName(ConstData.Table_LinkedTitles_Code);
            this.Property(l => l.Title).HasColumnName(ConstData.Table_LinkedTitles_Title);
            this.Property(l => l.Type).HasColumnName(ConstData.Table_LinkedTitles_Type);
        }
    }
}
