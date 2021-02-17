using ICD.DataAccess.Mapping;
using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Class providing data from the ICD Database in the desired resulting format of Lists of IcdCode Objects
    /// </summary>
    public class ICDSqliteProvider : IICDDataProvider
    {
        /// <summary>
        /// The Database Context to connect to
        /// </summary>
        private IICDContext m_DbContext;

        /// <summary>
        /// Initiate a new DataProvider instance with the given Database Context
        /// </summary>
        /// <param name="dbContext">Database Context to utilize in the connection</param>
        public ICDSqliteProvider(IICDContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext is Null");
            m_DbContext = dbContext;
        }

        /// <summary>
        /// Table name for the IcdTerm FullTextSearch table
        /// </summary>
        private static readonly string Table_TermsFts = "SearchIcd_TermsFts";
        /// <summary>
        /// Table name for the IcdLinkedTitle FullTextSearch table
        /// </summary>
        private static readonly string Table_LinkedTitlesFts = "SearchIcd_LinkedTitlesFts";

        /// <summary>
        /// Get whole word exact searches with the IcdTerm FullTextSearch table
        /// </summary>
        /// <param name="terms">Space separated terms to use</param>
        /// <returns></returns>
        private IEnumerable<ObjectModels.IcdCode> GetWholeCodes(string terms)
        {
            var tempTable = String.Format(
                "SELECT DISTINCT {0}.{3} AS Title, {0}.{4} AS Code, {0}.{5} AS CodeType, '{6}' AS RetrievedFrom, '{7}' AS ChildType " +
                    "FROM {1} " +
                        "JOIN {0} " +
                            "ON {0}.{2} = {1}.docid " +
                    "WHERE {1} MATCH @searchFor " +
                    "ORDER BY {0}.{3};",
                ConstData.Table_Terms, Table_TermsFts,
                ConstData.Table_Terms_Id, ConstData.Table_Terms_Title, ConstData.Table_Terms_Code, ConstData.Table_Terms_Type,
                IcdCodeStrings.RetrievedFrom_WholeWord, IcdCodeStrings.ChildType_Parent);

            var matches = m_DbContext.RawQuery<IcdCode>(
                                    tempTable,
                                    new SQLiteParameter("@searchFor", terms))
                                .AsEnumerable();
            return matches;
        }

        private IEnumerable<IcdCode> GetWholeIndexCodes(string terms)
        {
            var tempTable = String.Format(
                "SELECT DISTINCT {0}.{5} AS Title, {0}.{4} AS Code, {0}.{3} AS CodeType, '{8}' AS RetrievedFrom, '{9}' AS ChildType " +
                    "FROM {1} " +
                        "JOIN {2}, {0} " +
                            "ON {1}.docid = {2}.{6} " +
                                "AND {0}.{4} = {2}.{7} " +
                    "WHERE {1} MATCH @searchFor " +
                    "ORDER BY {0}.{5};",
                ConstData.Table_Terms, Table_LinkedTitlesFts, ConstData.Table_LinkedTitles,
                ConstData.Table_Terms_Type, ConstData.Table_Terms_Code, ConstData.Table_Terms_Title,
                ConstData.Table_LinkedTitles_Id, ConstData.Table_LinkedTitles_Code,
                IcdCodeStrings.RetrievedFrom_WholeWordSimilar, IcdCodeStrings.ChildType_Parent);

            var indexMatches = m_DbContext.RawQuery<IcdCode>(
                                    tempTable,
                                    new SQLiteParameter("@searchFor", terms))
                                .AsEnumerable();
            return indexMatches;
        }

        private IEnumerable<IcdCode> GetPartialCodes(IEnumerable<string> terms)
        {
            var partialMatches = (from term in m_DbContext.IcdTerms
                                  where terms.All(s => term.Title.Contains(s))
                                  orderby term.Title
                                  select new IcdCode()
                                  {
                                      RetrievedFrom = IcdCodeStrings.RetrievedFrom_PartialWord,
                                      Code = term.Code,
                                      CodeType = term.Type,
                                      ChildType = IcdCodeStrings.ChildType_Parent,
                                      Title = term.Title
                                  })
                                  .AsEnumerable();
            return partialMatches;
        }

        private IEnumerable<IcdCode> GetPartialIndexCodes(IEnumerable<string> terms)
        {
            var indexMatchesPartial = (from linked in m_DbContext.IcdLinkedTitles
                                       join term in m_DbContext.IcdTerms on linked.Code equals term.Code
                                       where terms.All(s => linked.Title.Contains(s))
                                       orderby term.Title
                                       select new IcdCode()
                                       {
                                           RetrievedFrom = IcdCodeStrings.RetrievedFrom_PartialWordSimilar,
                                           Code = term.Code,
                                           CodeType = term.Type,
                                           ChildType = IcdCodeStrings.ChildType_Parent,
                                           Title = term.Title
                                       })
                                       .AsEnumerable();
            return indexMatchesPartial;
        }

        /// <summary>
        /// Perform a search using a given string of terms and get the results
        /// The terms are to be expected to be parsed by this Data Provider for appropriate uses
        /// </summary>
        /// <param name="terms">The terms used to perform a search</param>
        /// <param name="skip">Number of results to skip (used for pagination)</param>
        /// <param name="take">Number of results to take after skipped (used for pagination)</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects</returns>
        public List<ObjectModels.IcdCode> GetSearch(string terms, int skip, int take)
        {
            var returns = GetWholeCodes(terms).ToList();
            if (returns.Count < skip + take)
            {
                returns = returns.Union(GetWholeIndexCodes(terms), IcdCodeCodeEqualityComparer.Static).ToList();
                if (returns.Count < skip + take)
                {
                    var splitTerms = terms.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    returns = returns.Union(GetPartialCodes(splitTerms), IcdCodeCodeEqualityComparer.Static).ToList();
                    if (returns.Count < skip + take)
                    {
                        returns = returns.Union(GetPartialIndexCodes(splitTerms), IcdCodeCodeEqualityComparer.Static).ToList();
                    }
                }
            }
            return returns.Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Perform a search for a specific ICD-10 Code of a specific type
        /// </summary>
        /// <param name="code">ICD-10 Code to search for</param>
        /// <param name="codeType">What type of code ; most likely Diagnosis or Procedure</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects; expect 0-1 in normal conditions</returns>
        public List<IcdCode> GetCode(string code, string codeType)
        {
            var matches = from terms in m_DbContext.IcdTerms
                          where String.Equals(terms.Code, code) &&
                                String.Equals(terms.Type, codeType)
                          select new IcdCode()
                          {
                              ChildType = IcdCodeStrings.ChildType_Parent,
                              RetrievedFrom = IcdCodeStrings.RetrievedFrom_WholeWord,
                              Code = terms.Code,
                              CodeType = terms.Type,
                              Title = terms.Title
                          };
            return matches.ToList();
        }

        /// <summary>
        /// Get All codes of All types matching a specific ICD-10 Code
        /// </summary>
        /// <param name="code">ICD-10 Code to search for</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode Objects; expect 0-1 with a rare possibility of 2 in normal conditions</returns>
        public List<IcdCode> GetAllCode(string code)
        {
            var matches = from terms in m_DbContext.IcdTerms
                          where String.Equals(terms.Code, code)
                          select new IcdCode()
                          {
                              ChildType = IcdCodeStrings.ChildType_Parent,
                              RetrievedFrom = IcdCodeStrings.RetrievedFrom_WholeWord,
                              Code = terms.Code,
                              CodeType = terms.Type,
                              Title = terms.Title
                          };
            return matches.ToList();
        }

        /// <summary>
        /// Get all Children Codes for a given ICD-10 Code
        /// </summary>
        /// <param name="code">The ICD-10 Code to get the children of</param>
        /// <param name="codeType">The type of ICD-10 code that the previous code is; most likely Diagnosis or Procedure</param>
        /// <returns>List of all Children ICD-10 Codes, in IcdCode Objects; Includes all Types of Child codes</returns>
        public List<IcdCode> GetChildren(string code, string codeType)
        {
            var allAddCodes = (from addCode in m_DbContext.IcdAddCodes
                               join term in m_DbContext.IcdTerms on addCode.Code equals term.Code
                               where String.Equals(addCode.ParentCode, code) &&
                                     String.Equals(term.Type, codeType)
                               orderby term.Title
                               select new IcdCode()
                               {
                                   ChildType = addCode.AddType,
                                   Code = term.Code,
                                   CodeType = term.Type,
                                   Title = term.Title,
                                   RetrievedFrom = IcdCodeStrings.RetrievedFrom_Child
                               })
                               .AsEnumerable();

            var allDirectChildren = (from term in m_DbContext.IcdTerms
                                     where term.ParentCode.Equals(code)
                                     orderby term.Title
                                     select new IcdCode()
                                     {
                                         ChildType = IcdCodeStrings.ChildType_Direct,
                                         Code = term.Code,
                                         CodeType = term.Type,
                                         RetrievedFrom = IcdCodeStrings.RetrievedFrom_Child,
                                         Title = term.Title
                                     })
                                     .AsEnumerable();

            var orderedChildren =
                allAddCodes.Where(o => String.Equals(o.ChildType, IcdCodeStrings.ChildType_CodeFirst))
                           .Union(
                                allDirectChildren,
                                IcdCodeCodeEqualityComparer.Static)
                           .Union(
                                allAddCodes
                                    .Where(o => String.Equals(o.ChildType, IcdCodeStrings.ChildType_CodeAlso)),
                                IcdCodeCodeEqualityComparer.Static)
                           .Union(
                                allAddCodes
                                    .Where(o => String.Equals(o.ChildType, IcdCodeStrings.ChildType_CodeAdditional)),
                                IcdCodeCodeEqualityComparer.Static)
                           .Union(
                                allAddCodes
                                    .Where(o => String.Equals(o.ChildType, IcdCodeStrings.ChildType_Excludes1)),
                                IcdCodeCodeEqualityComparer.Static)
                           .Union(
                                allAddCodes
                                    .Where(o => String.Equals(o.ChildType, IcdCodeStrings.ChildType_Excludes2)),
                                IcdCodeCodeEqualityComparer.Static);

            return orderedChildren.ToList();
        }

        /// <summary>
        /// Get a specialized list held by the database
        /// This includes ICD-10 Codes and Dividers (more types to be supported in future?)
        /// </summary>
        /// <param name="listId">The Identifier of the list to retrieve</param>
        /// <returns>List of all IcdCode objects making up the requested list; May return 0 items if no matching list is found</returns>
        public List<IcdCode> GetList(string listId)
        {
            var ret = (from listItem in m_DbContext.IcdListItems
                       join term in m_DbContext.IcdTerms on listItem.Data equals term.Code into termItems
                       from termItem in termItems.DefaultIfEmpty()
                       where listItem.ListId.Equals(listId)
                       orderby listItem.ListOrder
                       select new IcdCode()
                       {
                           Title = (listItem.Type.Equals("Code") ? termItem.Title : listItem.Data),
                           Code = (listItem.Type.Equals("Code") ? termItem.Code : String.Empty),
                           CodeType = (listItem.Type.Equals("Code") ? termItem.Type : IcdCodeStrings.CodeType_Divider),
                           RetrievedFrom = IcdCodeStrings.RetrievedFrom_WholeWord,
                           ChildType = IcdCodeStrings.ChildType_Parent
                       }).ToList();

            return ret;
        }
    }
}
