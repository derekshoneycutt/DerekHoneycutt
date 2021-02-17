using System;
using System.Runtime.Serialization;

namespace NMS.Platform.Objects
{
    /// <summary>
    /// Describes a Session on the NMS Service
    /// </summary>
    [DataContract]
    public class SessionInfo
    {
        /// <summary>
        /// The actual session token issued by NMS. This token is used in session credentials.
        /// </summary>
        [DataMember]
        public string SessionToken { get; set; }
        /// <summary>
        /// The date/time that this session's token will expire and no longer be usable.
        /// </summary>
        [DataMember]
        public DateTime SessionTokenExpirationTime { get; set; }
        /// <summary>
        /// The unique UID of the user associated with this session. This may be blank if the session has no user context.
        /// </summary>
        [DataMember]
        public int UserUID { get; set; }
        /// <summary>
        /// The first name of the user associated with this session. This may be blank if the session has no user context.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// The last name of the user associated with this session. This may be blank if the session has no user context.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// The unique UID of the organization that this session is associated with.
        /// </summary>
        [DataMember]
        public int OrganizationUID { get; set; }
        /// <summary>
        /// A value indicating the name of the NMS server that should be used in requests to calls to other NMS service interfaces.
        /// </summary>
        [DataMember]
        public string NMSRedirect { get; set; }
        /// <summary>
        /// This guid is populated with the Organization.NuanceHealthcareAccountGuid.
        /// </summary>
        [DataMember]
        public Guid OrganizationGuid { get; set; }
    }
}
