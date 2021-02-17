using System.ServiceModel;
using System.ServiceModel.Web;

namespace NMS.Platform.Services
{
    /// <summary>
    /// Interface for User Management Service on the NMS server
    /// </summary>
    [ServiceContract]
    public interface IUserManagement
    {
        /// <summary>
        /// Retrieves a single user record, by UID.
        /// </summary>
        /// <param name="userUID">UID of the user to retrieve</param>
        /// <returns>User object describing the requested user</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "Users/{userUID}")]
        Objects.User GetUser(string userUID);
    }
}
