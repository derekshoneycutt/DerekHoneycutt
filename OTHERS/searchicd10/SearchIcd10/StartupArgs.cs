using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Deployment.Application;
using System.Linq;
using System.Windows;

namespace SearchIcd10
{
    /// <summary>
    /// Class to handle Search ICD-10 Tool's Startup Arguments
    /// </summary>
    public class StartupArgs
    {
        private static readonly string FallbackHostUriDefault = "" /* Should actually fill this with something! */;

        /// <summary>
        /// Text specified to search for by arguments
        /// </summary>
        public string SearchForText { get; set; }

        /// <summary>
        /// Host URI to connect to for operations
        /// </summary>
        public string HostUri { get; set; }

        public string SessionToken { get; set; }

        /// <summary>
        /// Delegate for functions to try specific command line arguments
        /// </summary>
        /// <param name="obj">StartupArgs object to process argument into</param>
        /// <param name="args">List of command line arguments</param>
        /// <param name="argOn">Reference to index of currently searched argument</param>
        /// <returns>True if argument was successfully processed</returns>
        private delegate bool TryArgument(StartupArgs obj, List<string> args, ref int argOn);

        /// <summary>
        /// List of all functions used to process the command line arguments
        /// </summary>
        private static List<TryArgument> TryArgFuncs = new List<TryArgument>()
            {
                //Process argument specifying specific terms to search for
                (StartupArgs obj, List<string> args, ref int argOn) => 
                {
                    if (args[argOn].Equals("/SEARCHFOR", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (++argOn < args.Count)
                        {
                            obj.SearchForText = args[argOn];
                            return true;
                        }
                    }
                    return false;
                },

                //Process argument specifying a search from text on the clipboard
                (StartupArgs obj, List<string> args, ref int argOn) => 
                {
                    if (args[argOn].Equals("/SEARCHFORCLIP", StringComparison.CurrentCultureIgnoreCase))
                    {
                        obj.SearchForText = Clipboard.GetText(TextDataFormat.Text);
                        return true;
                    }
                    return false;
                },
                
                //Process argument specifying NMS Host to connect to for operation
                (StartupArgs obj, List<string> args, ref int argOn) => 
                {
                    if (args[argOn].Equals("/NMSHOST", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (++argOn < args.Count)
                        {
                            obj.HostUri = args[argOn];
                            return true;
                        }
                    }
                    return false;
                },
                
                //Process argument specifying Session Token to utilize
                (StartupArgs obj, List<string> args, ref int argOn) => 
                {
                    if (args[argOn].Equals("/SESSION", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (++argOn < args.Count)
                        {
                            obj.SessionToken = args[argOn];
                            return true;
                        }
                    }
                    return false;
                }
            };

        /// <summary>
        /// Initialize a StartupArgs object with an existing array of command-line arguments
        /// </summary>
        /// <param name="args">Command-line arguments to process</param>
        public StartupArgs(string[] args)
        {
            SessionToken = null;
            HostUri = ConfigurationManager.AppSettings["DefaultNmsAddr"];
            if (String.IsNullOrWhiteSpace(HostUri))
            {
                HostUri = FallbackHostUriDefault;
            }
            var startHostUri = HostUri;
            SearchForText = String.Empty;
            var trueArgs = args.ToList();

            for (int argOn = 0; argOn < trueArgs.Count; ++argOn)
            {
                foreach (var tryFunc in TryArgFuncs)
                {
                    if (tryFunc(this, trueArgs, ref argOn))
                    {
                        break;
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(HostUri))
            {
                HostUri = startHostUri;
            }

            var nameValues = GetQueryStringParameters();
            var launchParamsStr = nameValues.Get("LaunchParams");
            if (!String.IsNullOrEmpty(launchParamsStr))
            {
                var launchParams = nameValues.Get("LaunchParams").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                for (int argOn = 0; argOn < launchParams.Count; ++argOn)
                {
                    foreach (var tryFunc in TryArgFuncs)
                    {
                        if (tryFunc(this, launchParams, ref argOn))
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get parameters of the application in case the application is run on a ClickOnce deployment
        /// </summary>
        /// <returns>NameValueCollection that contains all of the parameters</returns>
        private NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection col = new NameValueCollection();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    if (ApplicationDeployment.CurrentDeployment == null)
                    {
                        return col;
                    }
                    if (ApplicationDeployment.CurrentDeployment.ActivationUri == null)
                    {
                        return col;
                    }

                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                    if (!String.IsNullOrEmpty(queryString))
                    {
                        col = System.Web.HttpUtility.ParseQueryString(queryString);
                    }
                }
                catch (UriFormatException)
                {
                    var domainArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                    if (domainArgs.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(domainArgs[0]))
                        {
                            col = System.Web.HttpUtility.ParseQueryString(domainArgs[0]);
                        }
                    }
                }
            }
            return col;
        }
    }
}
