using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Interface describing a Context to the ICD Database
    /// </summary>
    public interface IICDContext
    {
        /// <summary>
        /// Get the elements from the ICD Terms Table
        /// </summary>
        DbSet<IcdTerm> IcdTerms { get; }
        /// <summary>
        /// Get the elements from the ICD 'Similar' Words and other Linked Titles Table
        /// </summary>
        DbSet<IcdLinkedTitle> IcdLinkedTitles { get; }
        /// <summary>
        /// Get the elements from the Additional Child Codes Table
        /// </summary>
        DbSet<IcdAddCode> IcdAddCodes { get; }
        /// <summary>
        /// Get the elements from the Lists Table
        /// </summary>
        DbSet<IcdListItem> IcdListItems { get; }

        /// <summary>
        /// Executes a Raw Query on the Database and return 
        /// </summary>
        /// <typeparam name="T">Return type of objects</typeparam>
        /// <param name="query">The Raw Query to execute</param>
        /// <param name="parameters">Parameters to include in the query</param>
        /// <returns>Enumerable of returned results--May be a raw SQL query yet to be executed</returns>
        IEnumerable<T> RawQuery<T>(string query, params object[] parameters);
    }
}
