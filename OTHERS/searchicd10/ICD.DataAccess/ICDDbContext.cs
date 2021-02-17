using ICD.DataAccess.ObjectModels;
using ICD.DataAccess.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;

namespace ICD.DataAccess
{
    /// <summary>
    /// ICD Database Context connecting to SQL Server
    /// </summary>
    public partial class ICDDbContext : DbContext, IICDContext
    {
        protected static string m_connectionString = "";
        public static ICDDbContext Create()
        {
            return new ICDDbContext();
        }

        public static ICDDbContext CreateSqlite(string filename)
        {
            return new ICDDbContext(new System.Data.SQLite.SQLiteConnection(String.Format("Data Source={0}", filename)), true);
        }
        
        public ICDDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection) { }

        public ICDDbContext(string connString)
            : base(connString) { }

        public ICDDbContext() { }

        /// <summary>
        /// Get the elements from the ICD Terms Table
        /// </summary>
        public DbSet<IcdTerm> IcdTerms { get; set; }
        /// <summary>
        /// Get the elements from the ICD 'Similar' Words and other Linked Titles Table
        /// </summary>
        public DbSet<IcdLinkedTitle> IcdLinkedTitles { get; set; }
        /// <summary>
        /// Get the elements from the Additional Child Codes Table
        /// </summary>
        public DbSet<IcdAddCode> IcdAddCodes { get; set; }
        /// <summary>
        /// Get the elements from the Lists Table
        /// </summary>
        public DbSet<IcdListItem> IcdListItems { get; set; }

        /// <summary>
        /// Executes a Raw Query on the Database and return 
        /// </summary>
        /// <typeparam name="T">Return type of objects</typeparam>
        /// <param name="query">The Raw Query to execute</param>
        /// <param name="parameters">Parameters to include in the query</param>
        /// <returns>Enumerable of returned results--May be a raw SQL query yet to be executed</returns>
        public IEnumerable<T> RawQuery<T>(string query, params object[] parameters)
        {
            return Database.SqlQuery<T>(query, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                                .Add(new IcdTermModelMapper())
                                .Add(new IcdLinkedTitleModelMapper())
                                .Add(new IcdAddCodeModelMapper())
                                .Add(new IcdListItemModelMapper());
        }
    }
}
