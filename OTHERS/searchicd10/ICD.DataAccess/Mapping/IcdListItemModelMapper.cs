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
    /// Class used to map the IcdListItem object to a SQL Database
    /// </summary>
    public class IcdListItemModelMapper : EntityTypeConfiguration<IcdListItem>
    {
        public IcdListItemModelMapper()
        {
            //Table
            this.ToTable(ConstData.Table_Lists);

            //Primary Key
            this.HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName(ConstData.Table_Lists_Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            //Other Columns
            this.Property(t => t.Data).HasColumnName(ConstData.Table_Lists_Data);
            this.Property(t => t.ListId).HasColumnName(ConstData.Table_Lists_ListId);
            this.Property(t => t.ListOrder).HasColumnName(ConstData.Table_Lists_ListOrder);
            this.Property(t => t.Type).HasColumnName(ConstData.Table_Lists_Type);
        }
    }
}
