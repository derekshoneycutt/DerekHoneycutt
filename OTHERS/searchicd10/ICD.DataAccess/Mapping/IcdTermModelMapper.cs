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
    /// Class used to map the IcdTerm object to a SQL Database
    /// </summary>
    public class IcdTermModelMapper : EntityTypeConfiguration<IcdTerm>
    {
        public IcdTermModelMapper()
        {
            //Table
            this.ToTable(ConstData.Table_Terms);

            //Primary Key
            this.HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName(ConstData.Table_Terms_Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            //Other Columns
            this.Property(t => t.Code).HasColumnName(ConstData.Table_Terms_Code);
            this.Property(t => t.Section).HasColumnName(ConstData.Table_Terms_Section);
            this.Property(t => t.Title).HasColumnName(ConstData.Table_Terms_Title);
            this.Property(t => t.Type).HasColumnName(ConstData.Table_Terms_Type);
            this.Property(t => t.ParentCode).HasColumnName(ConstData.Table_Terms_ParentCode);
        }
    }
}
