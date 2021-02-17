using System;
using System.Runtime.InteropServices;

namespace VagabondLib.Win32.Internal.AppBar
{
    /// <summary>
    /// Exception thrown when AppBar Message failed without appropriate handling
    /// </summary>
    [Serializable]
    internal class AppBarMessageFailedException : Exception
    {
        public AppBarMessageFailedException()
            : base() { }
        public AppBarMessageFailedException(string message)
            : base(message) { }
        public AppBarMessageFailedException(string message, Exception innerException)
            : base(message, innerException) { }
        public AppBarMessageFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Handles an AppBar in Windows
    /// </summary>
    internal class AppBar
    {
        /// <summary>
        /// Application Bar Messages that can be sent
        /// </summary>
        private enum Message : uint
        {
            /// <summary>
            /// Registers a new appbar and specifies the message identifier that the system should use to send notification messages to the appbar.
            /// </summary>
            ABM_NEW = 0,
            /// <summary>
            /// Unregisters an appbar, removing the bar from the system's internal list.
            /// </summary>
            ABM_REMOVE = 1,
            /// <summary>
            /// Requests a size and screen position for an appbar.
            /// </summary>
            ABM_QUERYPOS = 2,
            /// <summary>
            /// Sets the size and screen position of an appbar.
            /// </summary>
            ABM_SETPOS = 3,
            /// <summary>
            /// Retrieves the autohide and always-on-top states of the Windows taskbar.
            /// </summary>
            ABM_GETSTATE = 4,
            /// <summary>
            /// Retrieves the bounding rectangle of the Windows taskbar. 
            /// Note that this applies only to the system taskbar. 
            /// Other objects, particularly toolbars supplied with third-party software, also can be present. 
            /// As a result, some of the screen area not covered by the Windows taskbar might not be visible to the user. 
            /// To retrieve the area of the screen not covered by both the taskbar and other app bars—the working area available to your application—, 
            /// use the GetMonitorInfo function.
            /// </summary>
            ABM_GETTASKBARPOS = 5,
            /// <summary>
            /// Notifies the system to activate or deactivate an appbar. The lParam member of the APPBARDATA pointed to by pData is set to TRUE to activate or FALSE to deactivate.
            /// </summary>
            ABM_ACTIVATE = 6,
            /// <summary>
            /// Retrieves the handle to the autohide appbar associated with a particular edge of the screen.
            /// </summary>
            ABM_GETAUTOHIDEBAR = 7,
            /// <summary>
            /// Registers or unregisters an autohide appbar for an edge of the screen.
            /// </summary>
            ABM_SETAUTOHIDEBAR = 8,
            /// <summary>
            /// Notifies the system when an appbar's position has changed.
            /// </summary>
            ABM_WINDOWPOSCHANGED = 9,
            /// <summary>
            /// Windows XP and later: Sets the state of the appbar's autohide and always-on-top attributes.
            /// </summary>
            ABM_SETSTATE = 10,
            ABM_GETAUTOHIDEBAREX = 11,
            ABM_SETAUTOHIDEBAREX = 12
        }

        /// <summary>
        /// Contains information about a system appbar message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Data
        {
            /// <summary>
            /// The size of the structure, in bytes.
            /// </summary>
            public int cbSize; // initialize this field using: Marshal.SizeOf(typeof(Data));
            /// <summary>
            /// The handle to the appbar window.
            /// </summary>
            public IntPtr hWnd;
            /// <summary>
            /// An application-defined message identifier. 
            /// The application uses the specified identifier for notification messages that it sends to the appbar identified by the hWnd member. 
            /// This member is used when sending the ABM_NEW message.
            /// </summary>
            public uint uCallbackMessage;
            /// <summary>
            /// A value that specifies an edge of the screen.
            /// </summary>
            public uint uEdge;
            /// <summary>
            /// A RECT structure whose use varies depending on the message:
            /// </summary>
            public RECT rc;
            /// <summary>
            /// A message-dependent value.
            /// </summary>
            public int lParam;
        }

        private class NativeMethods
        {
            /// <summary>
            /// Sends an appbar message to the system.
            /// </summary>
            /// <param name="dwMessage">Appbar message value to send.</param>
            /// <param name="pData">A pointer to an APPBARDATA structure. The content of the structure on entry and on exit depends on the value set in the dwMessage parameter.</param>
            /// <returns>This function returns a message-dependent value.</returns>
            [DllImport("shell32.dll", EntryPoint = "SHAppBarMessage")]
            public static extern IntPtr SHAppBarMessage(uint dwMessage, ref Data pData);
        }

        /// <summary>
        /// Application Bar Notification constants
        /// </summary>
        public enum Notify : uint
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }

        /// <summary>
        /// Represents the state of the Taskbar
        /// </summary>
        public enum TaskbarState : uint
        {
            ABS_NONE = 0x00,
            ABS_AUTOHIDE = 0x01,
            ABS_ALWAYSONTOP = 0x02, //Not returned with Win7+
            ABS_BOTH = 0x03
        }

        private Data appbarData;

        public IntPtr hWnd
        {
            get
            {
                return appbarData.hWnd;
            }
        }

        /// <summary>
        /// Begin handling an AppBar for a Window, according to its handle
        /// </summary>
        /// <param name="inWnd">Window that will be associated to an AppBar</param>
        public AppBar(IntPtr inWnd)
        {
            appbarData = new Data();
            appbarData.cbSize = Marshal.SizeOf(typeof(Data));
            appbarData.hWnd = inWnd;
        }

        /// <summary>
        /// Registers a new appbar and specifies the message identifier that the system should use to send it notification messages. 
        /// This should be called before any other function.
        /// </summary>
        /// <returns>TRUE if successful, or FALSE if an error occurs or if the appbar is already registered.</returns>
        public bool CreateNew(uint CallbackMessage)
        {
            appbarData.uCallbackMessage = CallbackMessage;

            IntPtr ret = NativeMethods.SHAppBarMessage((uint)Message.ABM_NEW, ref appbarData);
            if (ret == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Registers a new appbar and specifies the WM_USER message as the identifer that the system should use to send it notification messages.
        /// This should be called before any other function.
        /// </summary>
        /// <returns>TRUE if successful, or FALSE if an error occurs or if the appbar is already registered.</returns>
        public bool CreateNew()
        {
            return CreateNew((uint)WindowsMessages.USER);
        }

        /// <summary>
        /// Unregisters an appbar by removing it from the system's internal list.
        /// The system no longer sends notification messages to the appbar or prevents other applications from using the screen area used by the appbar.
        /// </summary>
        public void Remove()
        {
            NativeMethods.SHAppBarMessage((uint)Message.ABM_REMOVE, ref appbarData);
        }

        /// <summary>
        /// Requests a size and screen position for an appbar. 
        /// When the request is made, the message proposes a screen edge and a bounding rectangle for the appbar.
        /// The system adjusts the bounding rectangle so that the appbar does not interfere with the Windows taskbar or any other appbars.
        /// </summary>
        /// <param name="screenEdge">The screen edge proposed</param>
        /// <param name="screenRect">The bounding rectangle proposed</param>
        /// <returns>The adjusted bounding rectangle</returns>
        public System.Drawing.Rectangle QueryPos(ScreenEdge screenEdge, System.Drawing.Rectangle screenRect)
        {
            appbarData.uEdge = (uint)screenEdge;
            appbarData.rc = (RECT)screenRect;

            NativeMethods.SHAppBarMessage((uint)Message.ABM_QUERYPOS, ref appbarData);
            return (System.Drawing.Rectangle)appbarData.rc;
        }

        /// <summary>
        /// Sets the size and screen position of an appbar.
        /// The message specifies a screen edge and the bounding rectangle for the appbar.
        /// The system may adjust the bounding rectangle so that the appbar does not interfere with the Windows taskbar or any other appbars.
        /// </summary>
        /// <param name="screenEdge">The screen edge for the appbar</param>
        /// <param name="screenRect">The bounding rectangle for the appbar</param>
        /// <returns>The adjusted bounding rectangle</returns>
        public System.Drawing.Rectangle SetPos(ScreenEdge screenEdge, System.Drawing.Rectangle screenRect)
        {
            appbarData.uEdge = (uint)screenEdge;
            appbarData.rc = (RECT)screenRect;

            NativeMethods.SHAppBarMessage((uint)Message.ABM_SETPOS, ref appbarData);
            return (System.Drawing.Rectangle)appbarData.rc;
        }

        /// <summary>
        /// Notifies the system that an appbar has been activated. An appbar should call this in response to the WM_ACTIVATE message.
        /// </summary>
        public void Activate()
        {
            NativeMethods.SHAppBarMessage((uint)Message.ABM_ACTIVATE, ref appbarData);
        }

        /// <summary>
        /// Registers or unregisters an autohide appbar for a given edge of the screen.
        /// If the system has multiple monitors, the monitor that contains the primary taskbar is used.
        /// </summary>
        /// <param name="screenEdge">The edge of the screen to register the autohide appbar for</param>
        /// <param name="registerNew">Whether to register (true) or unregister (false) the autohide appbar</param>
        /// <returns>TRUE if successful, or FALSE if an error occurs or if an autohide appbar is already registered for the given edge.</returns>
        public bool SetAutoHideBar(ScreenEdge screenEdge, bool registerNew)
        {
            appbarData.uEdge = (uint)screenEdge;
            if (registerNew)
            {
                appbarData.lParam = 1;
            }
            else
            {
                appbarData.lParam = 0;
            }

            if (NativeMethods.SHAppBarMessage((uint)Message.ABM_SETAUTOHIDEBAR, ref appbarData) == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Registers or unregisters an autohide appbar for a given edge of the screen.
        /// This extends SetAutoHideBar by enabling you to specify a particular monitor, for use in multiple monitor situations.
        /// </summary>
        /// <param name="screenEdge">The edge of the screen to register the autohide appbar for</param>
        /// <param name="registerNew">Whether to register (true) or unregister (false) the autohide appbar</param>
        /// <param name="screenRect">Rectangle specifying the screen for the AutoHide Appbar</param>
        /// <returns>TRUE if successful, or FALSE if an error occurs or if an autohide appbar is already registered for the given edge on the given monitor.</returns>
        public bool SetAutoHideBarEx(ScreenEdge screenEdge, bool registerNew, System.Drawing.Rectangle screenRect)
        {
            appbarData.uEdge = (uint)screenEdge;
            appbarData.rc = (RECT)screenRect;
            if (registerNew)
            {
                appbarData.lParam = 1;
            }
            else
            {
                appbarData.lParam = 0;
            }

            if (NativeMethods.SHAppBarMessage((uint)Message.ABM_SETAUTOHIDEBAREX, ref appbarData) == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Notifies the system when an appbar's position has changed. An appbar should call this message in response to the WM_WINDOWPOSCHANGED message.
        /// </summary>
        public void NotifyPosChange()
        {
            NativeMethods.SHAppBarMessage((uint)Message.ABM_WINDOWPOSCHANGED, ref appbarData);
        }

        /// <summary>
        /// Sets the autohide and always-on-top states of the Windows taskbar.
        /// </summary>
        /// <param name="newState">The new state to set for the Taskbar</param>
        public void SetState(TaskbarState newState)
        {
            appbarData.lParam = (int)newState;

            NativeMethods.SHAppBarMessage((uint)Message.ABM_SETSTATE, ref appbarData);
        }

        /// <summary>
        /// Retrieves the bounding rectangle of the Windows taskbar.
        /// </summary>
        /// <returns>The bounding rectangle of the Windows taskbar.</returns>
        public System.Drawing.Rectangle GetTaskbarPos()
        {
            if (NativeMethods.SHAppBarMessage((uint)Message.ABM_GETTASKBARPOS, ref appbarData) == IntPtr.Zero)
            {
                throw new AppBarMessageFailedException("ABM_GETTASKBARPOS message returned false");
            }
            return (System.Drawing.Rectangle)appbarData.rc;
        }

        /// <summary>
        /// Retrieves the autohide and always-on-top states of the Windows taskbar.
        /// </summary>
        /// <returns>The current state of the Windows taskbar</returns>
        public static TaskbarState GetTaskbarState()
        {
            Data newData = new Data();
            newData.cbSize = Marshal.SizeOf(typeof(Data));

            return (TaskbarState)NativeMethods.SHAppBarMessage((uint)Message.ABM_GETSTATE, ref newData);
        }

        /// <summary>
        /// Retrieves the handle to the autohide appbar associated with an edge of the screen.
        /// If the system has multiple monitors, the monitor that contains the primary taskbar is used.
        /// </summary>
        /// <param name="screenEdge">The edge of the screen to retrieve the handle for</param>
        /// <returns>An object representing the autohide appbar, or NULL if an error occurs or if no autohide appbar is associated with the given edge.</returns>
        public static AppBar GetAutoHideBar(ScreenEdge screenEdge)
        {
            Data newData = new Data();
            newData.cbSize = Marshal.SizeOf(typeof(Data));
            newData.uEdge = (uint)screenEdge;

            IntPtr handle = NativeMethods.SHAppBarMessage((uint)Message.ABM_GETAUTOHIDEBAR, ref newData);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            return new AppBar(handle);
        }

        /// <summary>
        /// Retrieves the handle to the autohide appbar associated with an edge of the screen.
        /// This extends GetAutoHideBar by enabling you to specify a particular monitor, for use in multiple monitor situations.
        /// </summary>
        /// <param name="screenEdge">The edge of the screen to retrieve the handle for</param>
        /// <param name="screenRect">A Rectangle representing the screen to utilize</param>
        /// <returns>An object representing the autohide appbar, or NULL if an error occurs or if no autohide appbar is associated with the given edge of the given monitor.</returns>
        public static AppBar GetAutoHideBarEx(ScreenEdge screenEdge, System.Drawing.Rectangle screenRect)
        {
            Data newData = new Data();
            newData.cbSize = Marshal.SizeOf(typeof(Data));
            newData.uEdge = (uint)screenEdge;
            newData.rc = (RECT)screenRect;

            IntPtr handle = NativeMethods.SHAppBarMessage((uint)Message.ABM_GETAUTOHIDEBAREX, ref newData);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            return new AppBar(handle);
        }
    }
}
