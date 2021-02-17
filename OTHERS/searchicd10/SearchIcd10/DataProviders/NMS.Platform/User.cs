using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NMS.Platform.Objects
{
    /// <summary>
    /// Describes a user on the NMS
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// A unique identifier for this user.
        /// </summary>
        [DataMember]
        public int UID { get; set; }
        
        /// <summary>
        /// A Guid that uniquely identifies this user.
        /// </summary>
        [DataMember]
        public Guid NuanceUserGuid { get; set; }

        /// <summary>
        /// The UID of the account that this user belongs to.
        /// </summary>
        [DataMember]
        public int OrganizationUID { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// An optional middle name for the user.
        /// </summary>
        [DataMember]
        public string MiddleName { get; set; }

        /// <summary>
        /// An optional title for the user.
        /// </summary>
        [DataMember]
        public string Prefix { get; set; }

        /// <summary>
        /// An optional suffix for the user's name.
        /// </summary>
        [DataMember]
        public string Suffix { get; set; }

        /// <summary>
        /// The login that this user uses when logging into the NAS system.
        /// </summary>
        [DataMember]
        public string Login { get; set; }

        /// <summary>
        /// An optional email address for storing contact information for the user.
        /// </summary>
        [DataMember]
        public string EMailAddress { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string City { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string State { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }

        /// <summary>
        /// An address field for specifying contact information for the user.
        /// </summary>
        [DataMember]
        public string CountryCode { get; set; }

        /// <summary>
        /// An optional field to store application specific Department data.
        /// </summary>
        [DataMember]
        public string Department { get; set; }

        /// <summary>
        /// An optional field to store application specific Location data.
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Controls if a user account is disabled for login.
        /// </summary>
        [DataMember]
        public bool Disabled { get; set; }

        /// <summary>
        /// Controls if a user account is disabled for login.
        /// </summary>
        [DataMember]
        public bool DisableActiveDirectory { get; set; }

        /// <summary>
        /// The NPI of the Provider
        /// </summary>
        [DataMember]
        public string NPICode { get; set; }

        /// <summary>
        /// The Name of the Account
        /// </summary>
        [DataMember]
        public string AccountName { get; set; }

        /// <summary>
        /// Controls if a user account is enabled for sending message via email.
        /// </summary>
        [DataMember]
        public bool InformMeViaEmail { get; set; }

        /// <summary>
        /// Controls if a user account is enabled for sending message via SMS.
        /// </summary>
        [DataMember]
        public bool InformMeViaSMS { get; set; }

        /// <summary>
        /// Mobile Phone Number of the user.
        /// </summary>
        [DataMember]
        public string MobilePhoneNumber { get; set;}

        /// <summary>
        /// Mobile Phone provider of the user.
        /// </summary>
        [DataMember]
        public byte? MobilePhoneProvider { get; set; }

        /// <summary>
        /// Message Delivery type
        /// </summary>
        [DataMember]
        public byte? MessageDeliveryType { get; set; }

        /// <summary>
        /// The Date and time that this user was last modified, in UTC.
        /// </summary>
        [DataMember]
        public DateTime? DateLastAccess { get; set; }

        /// <summary>
        /// If true, this user was created from some kind of backend system credentials.
        /// </summary>
        [DataMember]
        public bool AutoProvisioned { get; set; }

        /// <summary>
        /// Number of groups the user is enrolled in. The term GroupEnrollmentCount must be included in the include list to retreive this field - it will not be populated with the default include list.
        /// </summary>
        [DataMember]
        public int GroupEnrollmentCount { get; set; }

        /// <summary>
        /// Type of user. Possible values are: User 0, Physician 1, Nurse 2, Resident 3, Physician Assistant	4, NMC Administrator 5, Other non physician 6
        /// </summary>
        [DataMember]
        public byte UserType { get; set; }

        /// <summary>
        /// Specifies whether the calling user is allowed to modify this user. This data is only populated for User queries when the caller specifically requests this field. It is blank otherwise.
        /// </summary>
        [DataMember]
        public string CanModify { get; set; }

        /// <summary>
        /// Specifies whether the calling user is allowed to delete this user. This data is only populated for User queries when the caller specifically requests this field. It is blank otherwise.
        /// </summary>
        [DataMember]
        public string CanDelete { get; set; }

        /// <summary>
        /// License GUIDs that are granted to the user
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> LicenseTypes { get; set; }

        /// <summary>
        /// Extension Data for the user
        /// </summary>
        [DataMember]
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
