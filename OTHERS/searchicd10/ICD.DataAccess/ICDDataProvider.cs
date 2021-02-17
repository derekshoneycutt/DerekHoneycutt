using ICD.DataAccess.Mapping;
using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Class providing data from the ICD Database in the desired resulting format of Lists of IcdCode Objects
    /// </summary>
    public class ICDDataProvider : IICDDataProvider
    {
        /// <summary>
        /// The Database Context to connect to
        /// </summary>
        private IICDContext m_DbContext;

        /// <summary>
        /// Initiate a new DataProvider instance with the given Database Context
        /// </summary>
        /// <param name="dbContext">Database Context to utilize in the connection</param>
        public ICDDataProvider(IICDContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext is Null");
            m_DbContext = dbContext;
        }

        /// <summary>
        /// Perform a search using a given string of terms and get the results
        /// The terms are to be expected to be parsed by this Data Provider for appropriate uses
        /// </summary>
        /// <param name="terms">The terms used to perform a search</param>
        /// <param name="skip">Number of results to skip (used for pagination)</param>
        /// <param name="take">Number of results to take after skipped (used for pagination)</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects</returns>
        public List<IcdCode> GetSearch(string terms, int skip, int take)
        {
            var splitTerms = terms.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var termsPairs = splitTerms.Select((s, index) =>
                new KeyValuePair<string, string>(String.Format("@key{0}", index), s));

            var tempTable = String.Format(
@"DECLARE @FtsForms AS varchar(1000)
SET @FtsForms = (SELECT 'FORMSOF(INFLECTIONAL, ' + {0} + ')')
DECLARE @highIndex AS INTEGER
SET @highIndex = @skipNum + @takeNum

CREATE TABLE #SearchIcdCodeSearch
   ({2} nvarchar(425),
   {3} nvarchar(10),
   {4} nvarchar(10),
   RetrievedFrom nvarchar(50),
   RFIndex int)

INSERT INTO #SearchIcdCodeSearch
   SELECT DISTINCT {2}, {3}, {4}, @WholeWordText AS RetrievedFrom, 0 AS RFIndex
       FROM {1}
       WHERE CONTAINS({2}, @FtsForms)

IF (SELECT COUNT(*) FROM #SearchIcdCodeSearch) < @highIndex
BEGIN
   MERGE #SearchIcdCodeSearch AS T
       USING
           (SELECT DISTINCT {1}.{2}, {1}.{3}, {1}.{4}, @WholeWordSimilarText AS RetrievedFrom, 1 AS RFIndex
               FROM {5}
                   JOIN {1}
                       ON {5}.{6} = {1}.{3}
               WHERE CONTAINS({5}.{7}, @FtsForms)) AS S
       ON (S.{3} = T.{3})
       WHEN NOT MATCHED BY TARGET THEN
           INSERT VALUES (S.{2}, S.{3}, S.{4}, S.RetrievedFrom, S.RFIndex);

   IF (SELECT COUNT(*) FROM #SearchIcdCodeSearch) < @highIndex
   BEGIN
       MERGE #SearchIcdCodeSearch AS T
           USING
               (SELECT {2}, {3}, {4}, @PartialWordText AS RetrievedFrom, 2 AS RFIndex
                   FROM {1}
                   WHERE {8}) AS S
           ON (S.{3} = T.{3})
           WHEN NOT MATCHED BY TARGET THEN
               INSERT VALUES (S.{2}, S.{3}, S.{4}, S.RetrievedFrom, S.RFIndex);

       IF (SELECT COUNT(*) FROM #SearchIcdCodeSearch) < @highIndex
       BEGIN
           MERGE #SearchIcdCodeSearch AS T
               USING
                   (SELECT DISTINCT {1}.{2}, {1}.{3}, {1}.{4}, @PartialWordSimilarText AS RetrievedFrom, 3 AS RFIndex
                       FROM {5}
                           JOIN {1}
                               ON {5}.{6} = {1}.{3}
                       WHERE {9}) AS S
               ON (S.{3} = T.{3})
               WHEN NOT MATCHED BY TARGET THEN
                   INSERT VALUES (S.{2}, S.{3}, S.{4}, S.RetrievedFrom, S.RFIndex);
       END
   END
END

SELECT {2} AS Title, {3} AS Code, {4} AS CodeType, RetrievedFrom, @ChildTypeText AS ChildType
   FROM (SELECT ROW_NUMBER() OVER (ORDER BY #SearchIcdCodeSearch.RFIndex, #SearchIcdCodeSearch.{2}) AS RowNum, #SearchIcdCodeSearch.*
               FROM #SearchIcdCodeSearch,
                    (SELECT MIN(RFIndex) AS RFIndex, #SearchIcdCodeSearch.{3}
                       FROM #SearchIcdCodeSearch
                       GROUP BY #SearchIcdCodeSearch.{3}) AS Grouped
               WHERE #SearchIcdCodeSearch.RFIndex = Grouped.RFIndex
                       AND #SearchIcdCodeSearch.{3} = Grouped.{3}) AS RowConstrained
   WHERE RowNum >= @skipNum + 1 AND RowNum <= @highIndex
   ORDER BY RowNum

DROP TABLE #SearchIcdCodeSearch;",
                String.Join(" + ') and FORMSOF(INFLECTIONAL, ' + ", termsPairs.Select(p => p.Key)),
                ConstData.Table_Terms,
                    ConstData.Table_Terms_Title, ConstData.Table_Terms_Code, ConstData.Table_Terms_Type,
                ConstData.Table_LinkedTitles,
                    ConstData.Table_LinkedTitles_Code, ConstData.Table_LinkedTitles_Title,
                String.Join(" AND ", termsPairs.Select(p =>
                                        String.Format("(CHARINDEX({0}, {1}) > 0)",
                                                    p.Key, ConstData.Table_Terms_Title))),
                String.Join(" AND ", termsPairs.Select(p =>
                                        String.Format("(CHARINDEX({0}, {1}.{2}) > 0)",
                                                    p.Key, ConstData.Table_LinkedTitles, ConstData.Table_LinkedTitles_Title))));

            var useParams = new List<SqlParameter>()
                {
                    new SqlParameter("@WholeWordText", IcdCodeStrings.RetrievedFrom_WholeWord),
                    new SqlParameter("@WholeWordSimilarText", IcdCodeStrings.RetrievedFrom_WholeWordSimilar),
                    new SqlParameter("@PartialWordText", IcdCodeStrings.RetrievedFrom_PartialWord),
                    new SqlParameter("@PartialWordSimilarText", IcdCodeStrings.RetrievedFrom_PartialWordSimilar),
                    new SqlParameter("@ChildTypeText", IcdCodeStrings.ChildType_Parent),
                    new SqlParameter("@skipNum", skip),
                    new SqlParameter("@takeNum", take)
                };

            useParams.AddRange(from pair in termsPairs
                               select new SqlParameter(pair.Key, pair.Value));

            var rawMatches = m_DbContext.RawQuery<IcdCode>(
                                            tempTable,
                                            useParams.ToArray())
                                        .ToList();

            return rawMatches;
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
