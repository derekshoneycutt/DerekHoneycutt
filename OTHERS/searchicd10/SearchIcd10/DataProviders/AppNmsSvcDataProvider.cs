using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace SearchIcd10.DataProviders
{
    /// <summary>
    /// Data Provider connecting to the NMS.ApplicationService.ICD service for performing Searches, etc.
    /// </summary>
    public class AppNmsSvcDataProvider : IAppDataProvider
    {
        /// <summary>
        /// Product GUID to use for the NMS connection
        /// </summary>
        private static readonly string NmsProductGUID = "ehhh" /* Need a real GUID here to work! */;
        /// <summary>
        /// License GUID that a user should have to use the Search ICD
        /// </summary>
        private static readonly Guid SearchIcdLicenseGuid = new Guid(/*Should actually have the GUID here*/);

        /// <summary>
        /// The host URL to use in connecting to the NMS for all operations
        /// </summary>
        private string m_HostUri;

        /// <summary>
        /// Session information for the currently used application session
        /// </summary>
        private NMS.Platform.Objects.SessionInfo m_SessInfo;

        /// <summary>
        /// Initialize a new NMS Application Service Data Provider with a given NMS Host URI
        /// </summary>
        /// <param name="hostUri">NMS Host URI to utilize in following calls</param>
        public AppNmsSvcDataProvider(string hostUri)
        {
            m_HostUri = hostUri;

            m_SessInfo = null;

        }

        /// <summary>
        /// Try an existing session, including validating for Search ICD license type
        /// </summary>
        /// <param name="startSession">Session token to try--if not null, will only try this token; otherwise, will try The "%AppData%/Nuance/Icd.Sess" file if it exists</param>
        /// <returns>True if session is valid for use with Search ICD; false otherwise</returns>
        public bool TryExistingSession(string startSession = null)
        {
            string sessionStr = startSession;

            if (sessionStr == null)
            {
                var sessFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Nuance", "Icd.Sess");
                if (File.Exists(sessFile))
                {
                    var encryptor = new SessionDataEncryptor();
                    sessionStr = encryptor.Decrypt(File.ReadAllText(sessFile));
                }
            }

            if (sessionStr != null)
            {
                var callGetSessions =
                    String.Format("{0}/NMS/Platform/SessionSvc/v1/",
                        m_HostUri);
                m_SessInfo = new NMS.Platform.Objects.SessionInfo();
                m_SessInfo.SessionToken = sessionStr;
                NMS.Platform.Objects.SessionInfo session = null;
                try
                {
                    session = CallSessionedREST<NMS.Platform.Objects.SessionInfo, NMS.Platform.ISessionSvc>(callGetSessions,
                        (proxy) =>
                        {
                            return proxy.RefreshSession();
                        });
                }
                finally
                {
                    m_SessInfo = null;
                }


                if (session != null)
                {
                    if (session.SessionTokenExpirationTime >= DateTime.Now.AddHours(-2))
                    {
                        m_SessInfo = session;
                        ValidateLicenseOnSession();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sets up and calls an NMS REST API with the session data stored in the HTTP header
        /// </summary>
        /// <typeparam name="TRet">Return type of the REST API call</typeparam>
        /// <typeparam name="TChannel">The type of Proxy Channel to create and utilize in the call</typeparam>
        /// <param name="url">URL of the call being made. (ie. "https://somethingsomethingsomething/NMS/etc/v1/")</param>
        /// <param name="callFunc">Function to call when the channel is created and HTTP headers properly initialized</param>
        /// <returns>The return of the callFunc function</returns>
        private TRet CallSessionedREST<TRet, TChannel>(string url, Func<TChannel, TRet> callFunc)
        {
            var factory = new ChannelFactory<TChannel>(new WebHttpBinding(WebHttpSecurityMode.Transport),
                new EndpointAddress(new Uri(url)));
            factory.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
            var channel = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)channel))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization",
                    String.Format("Basic {0}",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                String.Format("session//{0}:", m_SessInfo.SessionToken)))));
                return callFunc(channel);
            }
        }

        /// <summary>
        /// Close and Delete the current session that is opened, if applicable
        /// </summary>
        public void CloseSession()
        {
            if (m_SessInfo != null)
            {
                var callCloseUser =
                    String.Format("{0}/NMS/Platform/SessionSvc/v1/",
                        m_HostUri);
                CallSessionedREST<object, NMS.Platform.ISessionSvc>(callCloseUser,
                    (proxy) =>
                    {
                        proxy.CloseSession();
                        return null;
                    });
            }

            var sessFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Nuance", "Icd.Sess");
            if (File.Exists(sessFile))
            {
                File.Delete(sessFile);
            }
        }

        /// <summary>
        /// Validate that the current session is opened to a user with the Search ICD License granted
        /// <para>Throws InvalidLicenseException if user does not have the valid license</para>
        /// </summary>
        private void ValidateLicenseOnSession()
        {
            var callGetUser =
                String.Format("{0}/NMS/Platform/UserManagementSvc/v1/",
                    m_HostUri);
            CallSessionedREST<object, NMS.Platform.Services.IUserManagement>(callGetUser,
                (proxy) =>
                {
                    var user = proxy.GetUser(m_SessInfo.UserUID.ToString());

                    if (user.LicenseTypes == null)
                    {
                        CloseSession();
                        throw new InvalidLicenseException("No licenses retrieved for user");
                    }

                    if (!user.LicenseTypes.Contains(SearchIcdLicenseGuid))
                    {
                        CloseSession();
                        throw new InvalidLicenseException("User does not have appropriate license");
                    }
                    return null;
                });
        }

        /// <summary>
        /// Open a new Session for working with
        /// </summary>
        /// <param name="username">Username to login with on a new session</param>
        /// <param name="password">Passowrd to login with on a new session</param>
        public void NewSession(string username, string password)
        {
            var callUrl =
                String.Format("{0}/NMS/Platform/AuthenticationSvc/v1/",
                    m_HostUri);
            var factory = new ChannelFactory<NMS.Platform.Services.IAuthentication>(new WebHttpBinding(WebHttpSecurityMode.Transport),
                new EndpointAddress(new Uri(callUrl)));
            factory.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
            var channel = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)channel))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization",
                    String.Format("Basic {0}",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                String.Format("nms//{0}:{1}", username, password)))));
                m_SessInfo = channel.ValidateCredentials(null, NmsProductGUID);
            }

            ValidateLicenseOnSession();

            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Nuance");
            if (!Directory.Exists(appData))
            {
                Directory.CreateDirectory(appData);
            }
            var sessFile = Path.Combine(appData, "Icd.Sess");
            var encryptor = new SessionDataEncryptor();
            File.WriteAllText(sessFile, encryptor.Encrypt(m_SessInfo.SessionToken));
        }

        /// <summary>
        /// Make a call to the NMS.ApplicationService.ICD REST APIs
        /// </summary>
        /// <typeparam name="T">Type that the call will return</typeparam>
        /// <param name="getData">Function to call when connection established and headers properly set</param>
        /// <returns>The return of the getData function</returns>
        private T GetIcd<T>(Func<IICDDataService, T> getData)
        {
            var requestUrl =
                String.Format("{0}/NMS/Application/ICD/SearchSvc/v1/",
                    m_HostUri);
            return CallSessionedREST<T, IICDDataService>(requestUrl,
                (proxy) =>
                {
                    return getData(proxy);
                });
        }

        /// <summary>
        /// Perform a search using a given string of terms and get the results
        /// <para>The terms are to be expected to be parsed by this Data Provider for appropriate uses</para>
        /// </summary>
        /// <param name="terms">The terms used to perform a search</param>
        /// <param name="skip">Number of results to skip (used for pagination)</param>
        /// <param name="take">Number of results to take after skipped (used for pagination)</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects</returns>
        public List<IcdCode> GetSearch(string terms, int skip, int take)
        {
            return GetIcd(ids => ids.GetSearch(terms, skip, take));
        }

        /// <summary>
        /// Perform a search for a specific ICD-10 Code of a specific type
        /// </summary>
        /// <param name="code">ICD-10 Code to search for</param>
        /// <param name="codeType">What type of code ; most likely Diagnosis or Procedure</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode objects; expect 0-1 in normal conditions</returns>
        public List<IcdCode> GetCode(string code, string codeType)
        {
            return GetIcd(ids => ids.GetCode(code, codeType));
        }

        /// <summary>
        /// Get All codes of All types matching a specific ICD-10 Code
        /// </summary>
        /// <param name="code">ICD-10 Code to search for</param>
        /// <returns>List of all matching ICD-10 Codes, in IcdCode Objects; expect 0-1 with a rare possibility of 2 in normal conditions</returns>
        public List<IcdCode> GetAllCode(string code)
        {
            return GetIcd(ids => ids.GetAllCode(code));
        }

        /// <summary>
        /// Get all Children Codes for a given ICD-10 Code
        /// </summary>
        /// <param name="code">The ICD-10 Code to get the children of</param>
        /// <param name="codeType">The type of ICD-10 code that the previous code is; most likely Diagnosis or Procedure</param>
        /// <returns>List of all Children ICD-10 Codes, in IcdCode Objects; Includes all Types of Child codes</returns>
        public List<IcdCode> GetChildren(string code, string codeType)
        {
            return GetIcd(ids => ids.GetChildren(code, codeType));
        }

        /// <summary>
        /// Get a specialized list held by the database
        /// <para>This includes ICD-10 Codes and Dividers (more types to be supported in future?)</para>
        /// </summary>
        /// <param name="listId">The Identifier of the list to retrieve</param>
        /// <returns>List of all IcdCode objects making up the requested list; May return 0 items if no matching list is found</returns>
        public List<IcdCode> GetList(string listId)
        {
            return GetIcd(ids => ids.GetList(listId));
        }
    }
}
