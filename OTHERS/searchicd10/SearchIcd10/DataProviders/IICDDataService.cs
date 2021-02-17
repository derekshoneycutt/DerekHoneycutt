using ICD.DataAccess.ObjectModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SearchIcd10.DataProviders
{
    /// <summary>
    /// Provides an interface for querying ICD data.
    /// </summary>
    [ServiceContract]
    public interface IICDDataService
    {
        /// <summary>
        /// Searching ICD Codes is done by space-separated terms that are searched individually for each in an AND search for each
        /// </summary>
        /// <param name="terms" required="true">Search terms to search by</param>
        /// <param name="skip" validvalues="(>= 0)" required="true">The number of rows to skip from starting row.</param>
        /// <param name="take" validvalues="(>= 1)" required="true">Max number of items to return in the collection.</param>
        /// <returns>The collection of ICD Codes that match the Search terms </returns>
        [Description("Gets list of search results from ICD Codes")]
        [OperationContract]
        [WebGet(UriTemplate = "Search?terms={terms}&skip={skip}&take={take}")]
        List<IcdCode> GetSearch(string terms, int skip, int take);

        /// <summary>
        /// Data about a specific ICD Code is retrieved according to its Code and Type
        /// </summary>
        /// <param name="code" required="true">The ICD Code to get specific data about</param>
        /// <param name="codeType" validvalues="Diagnosis,Procedure" required="true">The type of code (Diagnosis/Procedure)</param>
        /// <returns>A collection of ICD Codes that match the requested code and type</returns>
        [Description("Gets matching ICD Code Data")]
        [OperationContract]
        [WebGet(UriTemplate = "Code?code={code}&codeType={codeType}")]
        List<IcdCode> GetCode(string code, string codeType);

        /// <summary>
        /// Data about specific ICD Codes is retrieved according to its Code
        /// <para>This may return 2 codes: 1 from both Procedure and Diagnosis each if they match</para>
        /// </summary>
        /// <param name="code" required="true">The ICD Code to get specific data about</param>
        /// <returns>A collection of ICD Codes that match the requested code</returns>
        [Description("Gets matching ICD Code Data")]
        [OperationContract]
        [WebGet(UriTemplate = "AllCode?code={code}")]
        List<IcdCode> GetAllCode(string code);

        /// <summary>
        /// Data about all first-level children of one ICD Code is retrieved
        /// <para>This returns both direct children and Additional Code Children</para>
        /// <para>For example, J45 Diagnosis may return J45.1 as well as Code First children</para>
        /// <para>J45 Diagnosis would not return J45.12</para>
        /// </summary>
        /// <param name="code" required="true">The ICD Code to get the children of</param>
        /// <param name="codeType" validvalues="Diagnosis,Procedure" required="true">The type of code (Diagnosis/Procedure)</param>
        /// <returns>A collection of ICD Code data that are the children of the requested code</returns>
        [Description("Gets Children of a specific ICD Code")]
        [OperationContract]
        [WebGet(UriTemplate = "Children?code={code}&codeType={codeType}")]
        List<IcdCode> GetChildren(string code, string codeType);

        /// <summary>
        /// Data about ICD Codes and Dividers on a list is retrieved
        /// </summary>
        /// <param name="listId" required="true">The string identifier of the list to retrieve</param>
        /// <returns>A collection of ICD Code data and Divider data that make up the requested list</returns>
        [Description("Gets a List of ICD Codes and Dividers")]
        [OperationContract]
        [WebGet(UriTemplate = "List?listId={listId}")]
        List<IcdCode> GetList(string listId);
    }
}
