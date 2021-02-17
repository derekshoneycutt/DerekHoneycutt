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
    /// Class used to map the IcdAddCode object to a SQL Database
    /// </summary>
    public class IcdAddCodeModelMapper : EntityTypeConfiguration<IcdAddCode>
    {
        public IcdAddCodeModelMapper()
        {
            //Table
            this.ToTable(ConstData.Table_AddCodes);

            //Primary Key
            this.HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName(ConstData.Table_AddCodes_Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            //Other Columns
            this.Property(t => t.AddType).HasColumnName(ConstData.Table_AddCodes_AddType);
            this.Property(t => t.Code).HasColumnName(ConstData.Table_AddCodes_Code);
            this.Property(t => t.ParentCode).HasColumnName(ConstData.Table_AddCodes_ParentCode);
            this.Property(t => t.Type).HasColumnName(ConstData.Table_AddCodes_Type);
        }
    }
}
