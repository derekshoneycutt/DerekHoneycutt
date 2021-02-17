using System;

namespace SearchIcd10.DataProviders
{
    /// <summary>
    /// Exception that is thrown if the user does not have an appropriate license for the Search ICD
    /// </summary>
    public class InvalidLicenseException : Exception
    {
        public InvalidLicenseException() { }

        public InvalidLicenseException(string message)
            : base(message) { }
    }
}
