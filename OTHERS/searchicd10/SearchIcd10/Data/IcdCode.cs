using System.Runtime.Serialization;

namespace sICD.DataAccess.ObjectModels
{
    /// <summary>
    /// Class to contain information about an ICD Code retrieved from a search
    /// </summary>
    [DataContract]
    public class IcdCode
    {
        /// <summary>
        /// The ICD-10 Code's Textual Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// The ICD-10 Code itself (ex. A10, A10.1234, 03E23, etc. NOTE: only 1 code should be represented here)
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// The type of ICD-10 Code
        /// See the IcdCodeStrings object for utilized values
        /// </summary>
        [DataMember]
        public string CodeType { get; set; }
        /// <summary>
        /// Where the Code was retrieved from; Known
        /// See the IcdCodeStrings object for utilized values
        /// </summary>
        [DataMember]
        public string RetrievedFrom { get; set; }
        /// <summary>
        /// The Type of child (possibly Parent) that this Code is considered from a GetChildren query
        /// See the IcdCodeStrings object for utilized values
        /// </summary>
        [DataMember]
        public string ChildType { get; set; }
    }
}
