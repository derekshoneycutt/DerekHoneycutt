using SearchIcd10.Utils;
using System;
using System.ComponentModel;
using System.ServiceModel.Security;
using System.ServiceModel.Web;
using System.Windows.Controls;
using System.Windows.Input;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// ViewModel of the Login screen for users
    /// </summary>
    public class LoginVM : NotifyingModel
    {
        public static readonly PropertyChangedEventArgs UserNameChangedArgs = new PropertyChangedEventArgs("UserName");
        public static readonly PropertyChangedEventArgs LoginFailChangedArgs = new PropertyChangedEventArgs("LoginFail");

        /// <summary>
        /// Event raised when the login is successful
        /// </summary>
        public event EventHandler LoginSuccess;

        private string m_UserName;
        /// <summary>
        /// Gets or Sets the UserName that the user wishes
        /// </summary>
        public string UserName
        {
            get { return m_UserName; }
            set { SetValue(ref m_UserName, value, UserNameChangedArgs); }
        }

        private string m_LoginFail;
        /// <summary>
        /// Gets or Sets a message when a login fails on the user
        /// </summary>
        public string LoginFail
        {
            get { return m_LoginFail; }
            set { SetValue(ref m_LoginFail, value, LoginFailChangedArgs); }
        }

        /// <summary>
        /// Command to run to test a login
        /// </summary>
        public ICommand TestLogin { get; set; }

        public LoginVM()
        {
            TestLogin = new Utils.ActionCommand<PasswordBox>(m_TestLogin);
            LoginFail = String.Empty;
        }

        /// <summary>
        /// Tests a login given the username and a passed PasswordBox
        /// </summary>
        /// <param name="pass">PasswordBox containing the user's password to utilize</param>
        private void m_TestLogin(PasswordBox pass)
        {
            try
            {
                App.DataProvider.NewSession(UserName, pass.Password);
                if (LoginSuccess != null)
                {
                    LoginSuccess(this, new EventArgs());
                }
            }
            catch (DataProviders.InvalidLicenseException)
            {
                LoginFail = "User does not have Search ICD License";
            }
            catch (WebFaultException<NMS.Platform.NMSExceptionDetail>)
            {
                LoginFail = "Unrecognized Username/Password";
            }
            catch (MessageSecurityException)
            {
                LoginFail = "Unrecognized Username/Password";
            }
        }
    }
}
