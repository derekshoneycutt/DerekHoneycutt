using ICD.DataAccess.ObjectModels;
using System.Collections.Generic;

namespace SearchIcd10.DataProviders
{
    /// <summary>
    /// Interface describing a Data Provider for the Application
    /// </summary>
    public interface IAppDataProvider
    {
        /// <summary>
        /// Attempt to try utilizing an existing session
        /// <para>May be given a session token to try, or will search in "%AppData%/Nuance/Icd.Sess" for a session token</para>
        /// </summary>
        /// <param name="startSession">Session token to try working with--may be null if not supplied outside of the AppData file</param>
        /// <returns>True if successful in utilizing existing session token</returns>
        bool TryExistingSession(string startSession = null);

        /// <summary>
        /// Close and Delete the current session that is opened, if applicable
        /// </summary>
        void CloseSession();

        /// <summary>
        /// Open a new Session for working with
        /// </summary>
        /// <param name="username">Username to login with on a new session</param>
        /// <param name="password">Passowrd to login with on a new session</param>
        void NewSession(string username, string password);

        /// <summary>
        /// Perform a search using a given string of terms and get the results
        /// <para>The terms are to be expected to be parsed by this Data Provider for appropriate uses</para>
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
        /// <para>This includes ICD-10 Codes and Dividers (more types to be supported in future?)</para>
        /// </summary>
        /// <param name="listId">The Identifier of the list to retrieve</param>
        /// <returns>List of all IcdCode objects making up the requested list; May return 0 items if no matching list is found</returns>
        List<IcdCode> GetList(string listId);
    }
}
