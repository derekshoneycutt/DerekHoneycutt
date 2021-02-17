using System.ServiceModel;
using System.ServiceModel.Web;

namespace NMS.Platform.Services
{
    /// <summary>
    /// Interface for the Authentication Service of the NMS Platform
    /// </summary>
    [ServiceContract]
    public interface IAuthentication
    {
        /// <summary>
        /// Verifies that the credentials passed in the authentication header of the request are valid. The user name used on the request should be one of the supported NMS user name formats( nms//, token// s2s//, sys//, ntlm//)
        /// </summary>
        /// <param name="location">Optional location data of the client (usually an IP address). Required for an application to work with dMIC.	</param>
        /// <param name="productGuid">The product guid of the calling application.</param>
        /// <returns>SessionInfo if successful at validating</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "ValidateCredentials?location={location}&productGuid={productGuid}")]
        Objects.SessionInfo ValidateCredentials(string location, string productGuid);
    }
}
