using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace NMS.Platform
{
    /// <summary>
    /// Interface for working with Sessions on the NMS Server
    /// </summary>
    [ServiceContract]
    public interface ISessionSvc
    {
        /// <summary>
        /// Closes the user session associated with the caller's session token.
        /// </summary>
        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "Sessions")]
        void CloseSession();

        /// <summary>
        /// Allows a user to retrieve non-secure information about their current active (non-expired) sessions for a particular product.
        /// </summary>
        /// <param name="productGuid">The product Guid for which to filter sessions by</param>
        /// <returns>Range of sessions open for the user and product GUID</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "Sessions?productGuid={productGuid}")]
        List<Objects.SessionInfo> GetSessions(string productGuid);

        /// <summary>
        /// Updates the expiration time of the token used in the call.
        /// </summary>
        /// <returns>Updated Session Information</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "Refresh")]
        Objects.SessionInfo RefreshSession();
    }
}
