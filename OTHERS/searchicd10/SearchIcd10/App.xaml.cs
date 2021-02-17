using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Windows;

namespace SearchIcd10
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static DataProviders.IAppDataProvider m_DataProvider;
        /// <summary>
        /// Data Provider to be used for getting all of the data to display
        /// </summary>
        public static DataProviders.IAppDataProvider DataProvider { get { return m_DataProvider; } }

        private static StartupArgs m_StartupArgs;
        /// <summary>
        /// Startup Arguments as processed for the application
        /// </summary>
        public static StartupArgs StartupArgs { get { return m_StartupArgs; } }

        private static bool m_RememberMe;
        /// <summary>
        /// Whether the user's successful session is to be remembered for the next time that they open the application
        /// </summary>
        public static bool RememberMe { get { return m_RememberMe; } }
        
        /// <summary>
        /// START POINT OF APPLICATION
        /// </summary>
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            m_RememberMe = false;
            m_StartupArgs = new StartupArgs(e.Args);

            m_DataProvider = new DataProviders.LocalSqliteDataProvider();
            //m_DataProvider = new DataProviders.AppNmsSvcDataProvider(StartupArgs.HostUri);

            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            bool exist = false;
            try
            {
                exist = DataProvider.TryExistingSession(m_StartupArgs.SessionToken);
                if (exist)
                {
                    m_RememberMe = true;
                }
            }
            catch (DataProviders.InvalidLicenseException)
            {
                if (StartupArgs.SessionToken != null)
                {
                    System.Windows.MessageBox.Show("User does not have Search ICD License");
                    Application.Current.Shutdown();
                }
                else
                {
                    exist = false;
                }
            }
            catch (WebFaultException<NMS.Platform.NMSExceptionDetail>)
            {
                exist = false;
            }
            catch (MessageSecurityException)
            {
                exist = false;
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                System.Windows.MessageBox.Show("The connection to the NMS server failed. Check your internet connection, or the server may be temporarily down.",
                                                "Search ICD-10", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            if (!exist)
            {
                var loginWin = new LoginWindow();
                loginWin.ShowDialog();
                if (loginWin.DialogResult != true)
                {
                    Application.Current.Shutdown();
                    return;
                }
            }

            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
            var listWin = new ListsWindow();
            listWin.Show();
            Application.Current.MainWindow = listWin;
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var endPointNotFound = e.Exception as System.ServiceModel.EndpointNotFoundException;
            if (endPointNotFound != null)
            {
                System.Windows.MessageBox.Show("The connection to the NMS server failed.\r\nCheck your internet connection, or the server may be temporarily down.",
                                                "Search ICD-10", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                Application.Current.Shutdown();
            }
        }
    }
}
