using System.Runtime.Serialization;

namespace NMS.Platform
{
    /// <summary>
    /// Enumeration of expected error codes from the NMSExceptionDetail
    /// </summary>
    [DataContract(Name = "ErrorCode")]
    public enum ExceptionErrorCode
    {
        [EnumMember]
        NoError = 0,
        [EnumMember]
        ParameterMissing = 1,
        [EnumMember]
        ParameterMalformed = 2,
        [EnumMember]
        ParameterInvalid = 3,
        [EnumMember]
        ParameterOutOfRange = 4,
        [EnumMember]
        StreamDataMissing = 5,
        [EnumMember]
        DataLayer = 6,
        [EnumMember]
        GeneralError = 7,
        [EnumMember]
        InvalidPwdLogon = 8,
        [EnumMember]
        InvalidCredentials = 9,
        [EnumMember]
        AccessDenied = 10,
        [EnumMember]
        PasswordExpired = 11,
        [EnumMember]
        InvalidPwdFormat = 12,
        [EnumMember]
        SerializationError = 13,
        [EnumMember]
        UserNotProvisioned = 14,
        /// <summary>
        /// A requested platform object could not be found
        /// </summary>
        [EnumMember]
        ObjectNotFound = 15,
        /// <summary>
        /// Used when a string parameter cannot be decoded to
        /// a byte array using a base 64 decoder.
        /// </summary>
        [EnumMember]
        ParameterStringNotBase64 = 16,
        /// <summary>
        /// License Key is invalid
        /// </summary>
        [EnumMember]
        InvalidLicenseKey = 17,
        /// <summary>
        /// License Key contains an undefined Organization
        /// </summary>
        [EnumMember]
        InvalidLicenseKeyUndefinedOrganization = 18,
        /// <summary>
        /// License Key contains an undefined Partner
        /// </summary>
        [EnumMember]
        InvalidLicenseKeyUndefinedPartner = 19,
        /// <summary>
        /// License Key contains an undefined Product
        /// </summary>
        [EnumMember]
        InvalidLicenseKeyUndefinedProduct = 20,
        /// <summary>
        /// License Key contains an undefined License Type
        /// </summary>
        [EnumMember]
        InvalidLicenseKeyUndefinedLicenseType = 21,
        /// <summary>
        /// License Key contains an undefined License Mode
        /// </summary>
        [EnumMember]
        InvalidLicenseKeyUndefinedLicenseMode = 22,
        /// <summary>
        /// An object being created cannot because one already
        /// exists with the data passed
        /// </summary>
        [EnumMember]
        ObjectAlreadyExists = 23,
        /// <summary>
        /// used when a DbEntityValidationException has occurred
        /// </summary>
        [EnumMember]
        DataValidationError = 24,
        /// <summary>
        /// Indicates that the Extended Error Code in the
        /// NMSExceptionDetail is in use.
        /// </summary>
        [EnumMember]
        SeeExtendedErrorCode = 25,
        [EnumMember]
        PasswordChangeFailedComplexityRequirement = 26,
        [EnumMember]
        PasswordChangeFailed = 27,
        /// <summary>
        /// The requested method is not implemented.
        /// </summary>
        [EnumMember]
        NotImplemented = 100,
    }

    /// <summary>
    /// Class describing an exception occurring on the NMS server
    /// </summary>
    [DataContract]
    public class NMSExceptionDetail
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public ExceptionErrorCode ErrorCode { get; set; }
    }
}
