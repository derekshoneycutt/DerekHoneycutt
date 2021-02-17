using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Interface describing a Data Provider for the ICD Searches
    /// </summary>
    public interface IICDDataProvider
    {
        /// <summary>
        /// Perform a search using a given string of terms and get the results
        /// The terms are to be expected to be parsed by this Data Provider for appropriate uses
        /// </summary>
        /// <param name="terms">The terms used to perform a search</param>
        /// <param name="skip">Number of results to skip (used for pagination)</param>
        /// <param name="take">Number of results to take after skipped (used for pagination)</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects</returns>
        List<IcdCode> GetSearch(string terms, int skip, int take);

        /// <summary>
        /// Perform a search for a specific ICD-10 Code of a specific type
        /// </summary>
        /// <param name="code">ICD-10 Code to search for</param>
        /// <param name="codeType">What type of code ; most likely Diagnosis or Procedure</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects; expect 0-1 in normal conditions</returns>
        List<IcdCode> GetCode(string code, string codeType);

        /// <summary>
        /// Get All codes of All types matching a specific ICD-10 Code
        /// </summary>
        /// <param name="code">ICD-10 Code to search for</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode Objects; expect 0-1 with a rare possibility of 2 in normal conditions</returns>
        List<IcdCode> GetAllCode(string code);

        /// <summary>
        /// Get all Children Codes for a given ICD-10 Code
        /// </summary>
        /// <param name="code">The ICD-10 Code to get the children of</param>
        /// <param name="codeType">The type of ICD-10 code that the previous code is; most likely Diagnosis or Procedure</param>
        /// <returns>List of all Children ICD-10 Codes, in IcdCode Objects; Includes all Types of Child codes</returns>
        List<IcdCode> GetChildren(string code, string codeType);

        /// <summary>
        /// Get a specialized list held by the database
        /// This includes ICD-10 Codes and Dividers (more types to be supported in future?)
        /// </summary>
        /// <param name="listId">The Identifier of the list to retrieve</param>
        /// <returns>List of all IcdCode objects making up the requested list; May return 0 items if no matching list is found</returns>
        List<IcdCode> GetList(string listId);
    }
}
