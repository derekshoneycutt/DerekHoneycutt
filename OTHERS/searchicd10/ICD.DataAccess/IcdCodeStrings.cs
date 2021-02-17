using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Static Class containing definitions of standard strings used by the ICD.DataAccess library
    /// </summary>
    public static class IcdCodeStrings
    {
        private static readonly string m_CodeType_Diagnosis = "Diagnosis";
        /// <summary>
        /// String representing the CodeType of Diagnosis
        /// Most returns may include this value
        /// </summary>
        public static string CodeType_Diagnosis { get { return m_CodeType_Diagnosis; } }

        private static readonly string m_CodeType_Procedure = "Procedure";
        /// <summary>
        /// String representing the CodeType of Procedure
        /// Most returns may include this value
        /// </summary>
        public static string CodeType_Procedure { get { return m_CodeType_Procedure; } }

        private static readonly string m_CodeType_Divider = "Divider";
        /// <summary>
        /// String representing the CodeType of Divider
        /// This is used in the creation of Organized Lists within the ICD Database
        /// </summary>
        public static string CodeType_Divider { get { return m_CodeType_Divider; } }

        private static readonly string m_RetrievedFrom_Child = "Child";
        /// <summary>
        /// String representing that an item was retrieved from a Child search
        /// </summary>
        public static string RetrievedFrom_Child { get { return m_RetrievedFrom_Child; } }

        private static readonly string m_RetrievedFrom_WholeWord = "WholeWord";
        /// <summary>
        /// String representing that an item was retrieved from a Whole Word Exact search
        /// In some queries, this may include Language-based Inflectional or Stemmed whole word matches
        /// </summary>
        public static string RetrievedFrom_WholeWord { get { return m_RetrievedFrom_WholeWord; } }

        private static readonly string m_RetrievedFrom_WholeWordSimilar = "WholeWordSimilar";
        /// <summary>
        /// String representing that an item was retrieved from a Whole Word Similar search
        /// May have matched against abbreviations, similar words, or any other 'Similar' type words
        /// </summary>
        public static string RetrievedFrom_WholeWordSimilar { get { return m_RetrievedFrom_WholeWordSimilar; } }

        private static readonly string m_RetrievedFrom_PartialWord = "PartialWord";
        /// <summary>
        /// String representing that an item was retrieved from a Partial Word Exact search
        /// </summary>
        public static string RetrievedFrom_PartialWord { get { return m_RetrievedFrom_PartialWord; } }

        private static readonly string m_RetrievedFrom_PartialWordSimilar = "PartialWordSimilar";
        /// <summary>
        /// String representing that an item was retrieved from a Partial Word Similar search
        /// May have matched against abbreviations, similar words, or any other 'Similar' type words
        /// </summary>
        public static string RetrievedFrom_PartialWordSimilar { get { return m_RetrievedFrom_PartialWordSimilar; } }

        private static readonly string m_ChildType_Parent = "Parent";
        /// <summary>
        /// String representing that an item is not a child, but is rather considered a Parent object
        /// </summary>
        public static string ChildType_Parent { get { return m_ChildType_Parent; } }

        private static readonly string m_ChildType_Direct = "Direct";
        /// <summary>
        /// String representing that an item was retrieved as a direct child to another code
        /// </summary>
        public static string ChildType_Direct { get { return m_ChildType_Direct; } }

        private static readonly string m_ChildType_CodeFirst = "CodeFirst";
        /// <summary>
        /// String representing that an item was retrieved as a "Code First" child to another code
        /// </summary>
        public static string ChildType_CodeFirst { get { return m_ChildType_CodeFirst; } }

        private static readonly string m_ChildType_CodeAdditional = "CodeAdditional";
        /// <summary>
        /// String representing that an item was retrieved as a "Code Additional" child to another code
        /// </summary>
        public static string ChildType_CodeAdditional { get { return m_ChildType_CodeAdditional; } }

        private static readonly string m_ChildType_CodeAlso = "CodeAlso";
        /// <summary>
        /// String representing that an item was retrieved as a "Code Also" child to another code
        /// </summary>
        public static string ChildType_CodeAlso { get { return m_ChildType_CodeAlso; } }

        private static readonly string m_ChildType_Excludes1 = "Excludes1";
        /// <summary>
        /// String representing that an iem was retrieved as a "Excludes 1" child to another code
        /// </summary>
        public static string ChildType_Excludes1 { get { return m_ChildType_Excludes1; } }

        private static readonly string m_ChildType_Excludes2 = "Excludes2";
        /// <summary>
        /// String representing that an iem was retrieved as a "Excludes 1" child to another code
        /// </summary>
        public static string ChildType_Excludes2 { get { return m_ChildType_Excludes2; } }

        private static readonly string m_ListId_TopUsed = "TopList";
        /// <summary>
        /// String representing the Identifier of the "Top Used" List that is expected in the database
        /// </summary>
        public static string ListId_TopUsed { get { return m_ListId_TopUsed; } }
    }
}
