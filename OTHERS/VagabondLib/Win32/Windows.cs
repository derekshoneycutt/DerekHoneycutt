using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Win32 APIs for handling Windows
    /// </summary>
    public static class Windows
    {
        /// <summary>
        /// Object used to send data through the EnumWindows API
        /// </summary>
        private class EnumObject
        {
            public IntPtr hWnd = IntPtr.Zero;
            public object Data = null;
        }
        /// <summary>
        /// Callback Delegate for the EnumWindows API
        /// </summary>
        /// <param name="hwnd">Current Window to evaluate</param>
        /// <param name="param">Object parameter passed to the original API call</param>
        /// <returns>True to continue processing</returns>
        private delegate bool EnumCallBack(IntPtr hwnd, ref EnumObject param);

        /// <summary>
        /// Handle representing the Top most window
        /// </summary>
        public static IntPtr HWND_TOPMOST { get { return (IntPtr)(-2); } }
        /// <summary>
        /// Handle representing the bottom window on the Z Order; used in SetWindowPosition, moves the window to the bottom of the Z Order
        /// </summary>
        public static IntPtr HWND_BOTTOM { get { return (IntPtr)(2); } }

        /// <summary>
        /// Flags defining the behavior of the SendMessageTimeout Function Messages
        /// </summary>
        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            /// <summary>
            /// The calling thread is not prevented from processing other requests while waiting for the function to return.
            /// </summary>
            SMTO_NORMAL = 0x0,
            /// <summary>
            /// Prevents the calling thread from processing any other requests until the function returns.
            /// </summary>
            SMTO_BLOCK = 0x1,
            /// <summary>
            /// The function returns without waiting for the time-out period to elapse if the receiving thread appears to not respond or "hangs."
            /// </summary>
            SMTO_ABORTIFHUNG = 0x2,
            /// <summary>
            /// The function does not enforce the time-out period as long as the receiving thread is processing messages.
            /// </summary>
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            /// <summary>
            /// The function should return 0 if the receiving window is destroyed or its owning thread dies while the message is being processed.
            /// </summary>
            SMTO_ERRORONEXIT = 0x0020
        }

        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        private static class NativeMethods
        {
            /// <summary>
            /// Retrieves the identifier of the thread that created the specified window and, optionally, the identifier of the process that created the window.
            /// </summary>
            /// <param name="hWnd">A handle to the window.</param>
            /// <param name="ProcessId">A pointer to a variable that receives the process identifier. If this parameter is not NULL, 
            ///     GetWindowThreadProcessId copies the identifier of the process to the variable; otherwise, it does not.</param>
            /// <returns>The identifier of the thread that created the window.</returns>
            [DllImport("user32.dll")]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

            /// <summary>
            /// Retrieves the name of the class to which the specified window belongs.
            /// </summary>
            /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
            /// <param name="lpClassName">The class name string.</param>
            /// <param name="nMaxCount">The length of the lpClassName buffer, in characters. The buffer must be large enough to include the terminating null character; 
            /// otherwise, the class name string is truncated to nMaxCount-1 characters.</param>
            /// <returns>The number of characters copied to the buffer, not including the terminating null character.</returns>
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetClassName(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpClassName, int nMaxCount);

            /// <summary>
            /// Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar). 
            /// If the specified window is a control, the function retrieves the length of the text within the control. 
            /// However, GetWindowTextLength cannot retrieve the length of the text of an edit control in another application.
            /// </summary>
            /// <param name="hWnd">A handle to the window or control.</param>
            /// <returns>The length, in characters, of the text. Under certain conditions, this value may actually be greater than the length of the text.</returns>
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            /// <summary>
            /// Copies the text of the specified window's title bar (if it has one) into a buffer.
            /// If the specified window is a control, the text of the control is copied.
            /// However, GetWindowText cannot retrieve the text of a control in another application.
            /// </summary>
            /// <param name="hWnd">A handle to the window or control containing the text.</param>
            /// <param name="lpString">The buffer that will receive the text. 
            /// If the string is as long or longer than the buffer, the string is truncated and terminated with a null character.</param>
            /// <param name="nMaxCount">The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is truncated.</param>
            /// <returns>The length, in characters, of the copied string, not including the terminating null character.
            /// If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero.</returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

            /// <summary>
            /// Retrieves the dimensions of the bounding rectangle of the specified window. 
            /// The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
            /// </summary>
            /// <param name="hWnd">A handle to the window.</param>
            /// <param name="rect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
            /// <returns>If the function succeeds, the return value is nonzero.</returns>
            [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
            public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

            /// <summary>
            /// Enumerates all top-level windows on the screen by passing the handle to each window, 
            /// in turn, to an application-defined callback function. 
            /// EnumWindows continues until the last top-level window is enumerated or the callback function returns FALSE.
            /// </summary>
            /// <param name="callPtr">A pointer to an application-defined callback function.</param>
            /// <param name="param">An application-defined value to be passed to the callback function.</param>
            /// <returns>If the function succeeds, the return value is nonzero.</returns>
            [DllImport("user32.dll")]
            public static extern int EnumWindows(EnumCallBack callPtr, ref EnumObject param);

            /// <summary>
            /// Enumerates the child windows that belong to the specified parent window by passing the handle to each child window, 
            /// in turn, to an application-defined callback function. 
            /// EnumChildWindows continues until the last child window is enumerated or the callback function returns FALSE.
            /// </summary>
            /// <param name="hwndParent">A handle to the parent window whose child windows are to be enumerated. If this parameter is NULL, this function is equivalent to EnumWindows.</param>
            /// <param name="lpEnumFunc">A pointer to an application-defined callback function.</param>
            /// <param name="lParam">An application-defined value to be passed to the callback function.</param>
            /// <returns>The return value is not used.</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumChildWindows(IntPtr hwndParent, EnumCallBack lpEnumFunc, ref EnumObject lParam);

            /// <summary>
            /// Sends the specified message to a window or windows. 
            /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
            /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms644950(v=vs.85).aspx
            /// </summary>
            /// <param name="hWnd">A handle to the window whose window procedure will receive the message. 
            /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, 
            /// including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
            /// <param name="Msg">The message to be sent.</param>
            /// <param name="wparam">Additional message-specific information.</param>
            /// <param name="lparam">Additional message-specific information.</param>
            /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
            [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam);

            /// <summary>
            /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
            /// </summary>
            /// <param name="lpString">The message to be registered.</param>
            /// <returns>A message identifier in the range 0xC000 through 0xFFFF.</returns>
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern int RegisterWindowMessage([MarshalAs(UnmanagedType.LPStr)] string lpString);

            /// <summary>
            /// Changes the size, position, and Z order of a child, pop-up, or top-level window. 
            /// These windows are ordered according to their appearance on the screen. 
            /// The topmost window receives the highest rank and is the first window in the Z order.
            /// </summary>
            /// <param name="hWnd">A handle to the window.</param>
            /// <param name="hWndInstertAfter">A handle to the window to precede the positioned window in the Z order.</param>
            /// <param name="x">The new position of the left side of the window, in client coordinates.</param>
            /// <param name="y">The new position of the top of the window, in client coordinates.</param>
            /// <param name="cx">The new width of the window, in pixels.</param>
            /// <param name="cy">The new height of the window, in pixels.</param>
            /// <param name="flags">The window sizing and positioning flags.</param>
            /// <returns>If the function succeeds, the return value is nonzero.</returns>
            [DllImportAttribute("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInstertAfter, int x, int y, int cx, int cy, SetWindowsPositionFlags flags);

            /// <summary>
            /// Retrieves a handle to the foreground window (the window with which the user is currently working). 
            /// The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
            /// </summary>
            /// <returns>A handle to the foreground window. The foreground window can be NULL in certain circumstances, such as when a window is losing activation.</returns>
            [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
            public static extern IntPtr GetForegroundWindow();


            /// <summary>
            /// Brings the thread that created the specified window into the foreground and activates the window.
            /// Keyboard input is directed to the window, and various visual cues are changed for the user.
            /// The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
            /// </summary>
            /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
            /// <returns>True if the window was brought to the foreground.</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            /// <summary>
            /// Brings the specified window to the top of the Z order. 
            /// If the window is a top-level window, it is activated. 
            /// If the window is a child window, the top-level parent window associated with the child window is activated.
            /// </summary>
            /// <param name="hWnd">A handle to the window to bring to the top of the Z order.</param>
            /// <returns>If the function succeeds, the return value is nonzero.</returns>
            [DllImport("user32.dll", SetLastError = true, EntryPoint = "BringWindowToTop")]
            public static extern bool BringToTop(IntPtr hWnd);

            /// <summary>
            /// Sends the specified message to one or more windows.
            /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms644952(v=vs.85).aspx
            /// </summary>
            /// <param name="hWnd">A handle to the window whose window procedure will receive the message.
            /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, 
            /// including disabled or invisible unowned windows. The function does not return until each window has timed out. 
            /// Therefore, the total wait time can be up to the value of uTimeout multiplied by the number of top-level windows.</param>
            /// <param name="Msg">The message to be sent.</param>
            /// <param name="wParam">Any additional message-specific information.</param>
            /// <param name="lParam">Any additional message-specific information.</param>
            /// <param name="flags">The behavior of this function.</param>
            /// <param name="timeout">The duration of the time-out period, in milliseconds. 
            /// If the message is a broadcast message, each window can use the full time-out period. 
            /// For example, if you specify a five second time-out period and there are three top-level windows that fail to process the message, you could have up to a 15 second delay.</param>
            /// <param name="result">The result of the message processing. The value of this parameter depends on the message that is specified.</param>
            /// <returns>If the function succeeds, the return value is nonzero. SendMessageTimeout does not provide information about individual windows timing out if HWND_BROADCAST is used.</returns>
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessageTimeout(
                IntPtr hWnd,
                WindowsMessages Msg,
                IntPtr wParam,
                IntPtr lParam,
                SendMessageTimeoutFlags flags,
                uint timeout,
                out IntPtr result);

            /// <summary>
            /// This version should be used specifically for WM_GETTEXT Message
            /// Sends the specified message to one or more windows.
            /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms644952(v=vs.85).aspx
            /// </summary>
            /// <param name="hWnd">A handle to the window whose window procedure will receive the message.
            /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, 
            /// including disabled or invisible unowned windows. The function does not return until each window has timed out. 
            /// Therefore, the total wait time can be up to the value of uTimeout multiplied by the number of top-level windows.</param>
            /// <param name="Msg">The message to be sent. Use WM_GETTEXT Here.</param>
            /// <param name="countOfChars">Any additional message-specific information -- The maximum number of characters to retrieve.</param>
            /// <param name="text">Any additional message-specific information -- The StringBuilder object to fill with retrieved text.</param>
            /// <param name="flags">The behavior of this function.</param>
            /// <param name="timeout">The duration of the time-out period, in milliseconds. 
            /// If the message is a broadcast message, each window can use the full time-out period. 
            /// For example, if you specify a five second time-out period and there are three top-level windows that fail to process the message, you could have up to a 15 second delay.</param>
            /// <param name="result">The result of the message processing. The value of this parameter depends on the message that is specified.</param>
            /// <returns>If the function succeeds, the return value is nonzero. SendMessageTimeout does not provide information about individual windows timing out if HWND_BROADCAST is used.</returns>
            [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern UIntPtr SendMessageTimeoutText(
                IntPtr hWnd,
                WindowsMessages Msg,              // Use WM_GETTEXT
                IntPtr countOfChars,
                [MarshalAs(UnmanagedType.LPWStr)] StringBuilder text,
                SendMessageTimeoutFlags flags,
                uint timeout,
                out IntPtr result);

            /// <summary>
            /// Sends the specified message to a window or windows. 
            /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
            /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms644950(v=vs.85).aspx
            /// </summary>
            /// <param name="hWnd">A handle to the window whose window procedure will receive the message. 
            /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, 
            /// including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
            /// <param name="Msg">The message to be sent.</param>
            /// <param name="wparam">Additional message-specific information.</param>
            /// <param name="lparam">Additional message-specific information.</param>
            /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wparam, IntPtr lparam);

            /// <summary>
            /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
            /// </summary>
            /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
            /// <param name="nIndex">The zero-based offset to the value to be retrieved. 
            /// Valid values are in the range zero through the number of bytes of extra window memory, minus four; 
            /// for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer. 
            /// To retrieve any other value, specify one of the WindowLongs values.</param>
            /// <returns>If the function succeeds, the return value is the requested value. If the function fails, the return value is zero.</returns>
            [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
            public static extern uint GetLong(IntPtr hWnd, WindowsLongs nIndex);

            /// <summary>
            /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
            /// </summary>
            /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
            /// <param name="nIndex">The zero-based offset to the value to be set. 
            /// Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
            /// <param name="dwNewLong">The replacement value.</param>
            /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
            [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
            public static extern int SetLong(IntPtr hWnd, WindowsLongs nIndex, uint dwNewLong);

            /// <summary>
            /// Determines the visibility state of the specified window.
            /// </summary>
            /// <param name="hWnd">A handle to the window to be tested.</param>
            /// <returns>If the specified window, its parent window, its parent's parent window, and so forth, have the WS_VISIBLE style, the return value is nonzero. 
            /// Otherwise, the return value is zero.</returns>
            [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
            public static extern bool IsVisible(IntPtr hWnd);

            /// <summary>
            /// Sets the specified window's show state.
            /// </summary>
            /// <param name="hWnd">A handle to the window.</param>
            /// <param name="nCmdShow">Controls how the window is to be shown.</param>
            /// <returns>If the window was previously visible, the return value is nonzero. If the window was previously hidden, the return value is zero.</returns>
            [DllImport("user32.dll", EntryPoint = "ShowWindow")]
            public static extern bool Show(IntPtr hWnd, WindowsShowCommands nCmdShow);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWindowVisible(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);
        }


        /// <summary>
        /// Get a handle to the foreground window of Windows
        /// </summary>
        /// <returns>A handle to the foreground window. Can be NULL in certain circumstances, such as when a window is losing activation</returns>
        public static IntPtr GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }

        public static IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd)
        {
            return NativeMethods.GetWindow(hWnd, uCmd);
        }

        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window.
        /// Keyboard input is directed to the window, and various visual cues are changed for the user.
        /// The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
        /// </summary>
        /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
        /// <returns>True if the window was brought to the foreground.</returns>
        public static bool SetForegroundWindow(IntPtr win)
        {
            return NativeMethods.SetForegroundWindow(win);
        }

        /// <summary>
        /// Brings the specified window to the top of the Z order. 
        /// If the window is a top-level window, it is activated. 
        /// If the window is a child window, the top-level parent window associated with the child window is activated.
        /// </summary>
        /// <param name="hWnd">A handle to the window to bring to the top of the Z order.</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        public static bool BringToTop(IntPtr win)
        {
            return NativeMethods.BringToTop(win);
        }

        /// <summary>
        /// Sends the specified message to a window or windows. 
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms644950(v=vs.85).aspx
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. 
        /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, 
        /// including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
        /// <param name="Msg">The message to be sent.</param>
        /// <param name="wparam">Additional message-specific information.</param>
        /// <param name="lparam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public static IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wparam, IntPtr lparam)
        {
            return NativeMethods.SendMessage(hWnd, Msg, wparam, lparam);
        }

        /// <summary>
        /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be retrieved. 
        /// Valid values are in the range zero through the number of bytes of extra window memory, minus four; 
        /// for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer. 
        /// To retrieve any other value, specify one of the WindowLongs values.</param>
        /// <returns>If the function succeeds, the return value is the requested value. If the function fails, the return value is zero.</returns>
        public static uint GetLong(IntPtr hWnd, WindowsLongs nIndex)
        {
            return NativeMethods.GetLong(hWnd, nIndex);
        }

        /// <summary>
        /// Sets information about the specified window
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be retrieved. 
        /// Valid values are in the range zero through the number of bytes of extra window memory, minus four; 
        /// for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer. 
        /// To retrieve any other value, specify one of the WindowLongs values.</param>
        /// <param name="newValue">The replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the value offset. If teh function fails, the return value is zero.</returns>
        public static int SetLong(IntPtr hWnd, WindowsLongs nIndex, uint newValue)
        {
            return NativeMethods.SetLong(hWnd, nIndex, newValue);
        }

        /// <summary>
        /// Get the rectangle that a window occupies on the screen
        /// </summary>
        /// <param name="hWnd">The window</param>
        /// <returns>The rectangle that the window occupies on the screen</returns>
        public static System.Drawing.Rectangle GetRectangle(IntPtr hWnd)
        {
            RECT ret = new RECT();
            NativeMethods.GetWindowRect(hWnd, out ret);
            return (System.Drawing.Rectangle)ret;
        }

        /// <summary>
        /// Get the class name of a window
        /// </summary>
        /// <param name="hWnd">The Window's handle to get the class name of</param>
        /// <returns>The Classname of the window; null if not able</returns>
        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder className = new StringBuilder(1024);
            if (NativeMethods.GetClassName(hWnd, className, className.Capacity) > 0)
            {
                return className.ToString();
            }
            return null;
        }

        /// <summary>
        /// Use the Win32 API to retrieve a Window's Text via the GetWindowText API call
        /// </summary>
        /// <param name="hWnd">The window handle to retrieve text form</param>
        /// <returns>The Window's Text, or null if not successful at retrieving text</returns>
        public static string GetText(IntPtr hWnd)
        {
            int captLen = 0;
            string caption = null;
            if ((captLen = NativeMethods.GetWindowTextLength(hWnd)) > 0)
            {
                StringBuilder captStr = new StringBuilder(captLen + 1);
                if (NativeMethods.GetWindowText(hWnd, captStr, captStr.Capacity) > 0)
                {
                    caption = captStr.ToString();
                }
            }
            return caption;
        }

        /// <summary>
        /// Get a control's text via the WM_GETTEXT message
        /// </summary>
        /// <param name="ctrlHandle">Handle to the control to get text from</param>
        /// <returns>The control's text</returns>
        public static string GetControlText(IntPtr ctrlHandle)
        {
            IntPtr res;
            NativeMethods.SendMessageTimeout(ctrlHandle, WindowsMessages.GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 250, out res);
            int length = (int)res;
            StringBuilder sb = new StringBuilder(length + 1);

            NativeMethods.SendMessageTimeoutText(ctrlHandle, WindowsMessages.GETTEXT, (IntPtr)sb.Capacity, sb, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 250, out res);
            return sb.ToString();
        }

        /// <summary>
        /// Get a control's Caption (text) via the WM_GETTEXT message
        /// </summary>
        /// <param name="ctrlHandle">Handle to the control to get text from</param>
        /// <returns>The control's text</returns>
        public static string GetCaption(IntPtr ctrlHandle)
        {
            NativeMethods.RegisterWindowMessage("WM_GETTEXT");
            return GetControlText(ctrlHandle);
        }

        /// <summary>
        /// Private function used to find a Window by it's Caption -- Callback for EnumWindows
        /// </summary>
        /// <param name="hWnd">The window currently being tested</param>
        /// <param name="param">EnumWindowsObject passed to EnumWindows</param>
        /// <returns>Whether to continue procesing windows or exit with this one</returns>
        private static bool FindByCaptCallback(IntPtr hWnd, ref EnumObject param)
        {
            if (param.Data is string)
            {
                if (GetControlText(hWnd).Equals((string)param.Data))
                {
                    param.hWnd = hWnd;
                    return false;
                }
            }
            else if (param.Data is Func<string, bool>)
            {
                Func<string, bool> testFunc = (Func<string, bool>)param.Data;
                if (testFunc(GetControlText(hWnd)))
                {
                    param.hWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Find a window by a matching Caption string
        /// </summary>
        /// <param name="caption">The caption that the window should match</param>
        /// <returns>The Handle of a matching window, if found</returns>
        public static IntPtr FindByCaption(string caption)
        {
            NativeMethods.RegisterWindowMessage("WM_GETTEXT");
            EnumObject endVals = new EnumObject();
            endVals.Data = caption;
            NativeMethods.EnumWindows(new EnumCallBack(FindByCaptCallback), ref endVals);
            return endVals.hWnd;
        }

        /// <summary>
        /// Find a window according to its caption, using a passed function
        /// </summary>
        /// <param name="testFunc">Function used to test the caption; takes Caption in, returns True if match</param>
        /// <returns>The handle of a matching window, if found</returns>
        public static IntPtr FindByCaption(Func<string, bool> testFunc)
        {
            NativeMethods.RegisterWindowMessage("WM_GETTEXT");
            EnumObject endVals = new EnumObject();
            endVals.Data = testFunc;
            NativeMethods.EnumWindows(new EnumCallBack(FindByCaptCallback), ref endVals);
            return endVals.hWnd;
        }

        /// <summary>
        /// Private function used to find a Window by it's ClassNAme -- Callback for EnumWindows
        /// </summary>
        /// <param name="hWnd">The window currently being tested</param>
        /// <param name="param">EnumWindowsObject passed to EnumWindows</param>
        /// <returns>Whether to continue procesing windows or exit with this one</returns>
        private static bool FindByClassCallback(IntPtr hWnd, ref EnumObject param)
        {
            if (param.Data is string)
            {
                if (GetClassName(hWnd).Equals((string)param.Data))
                {
                    param.hWnd = hWnd;
                    return false;
                }
            }
            else if (param.Data is Func<string, bool>)
            {
                Func<string, bool> testFunc = (Func<string, bool>)param.Data;
                if (testFunc(GetClassName(hWnd)))
                {
                    param.hWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Find a window by a matching ClassName string
        /// </summary>
        /// <param name="classname">The classname that the window should match</param>
        /// <returns>The Handle of a matching window, if found</returns>
        public static IntPtr FindByClassName(string classname)
        {
            EnumObject endVals = new EnumObject();
            endVals.Data = classname;
            NativeMethods.EnumWindows(new EnumCallBack(FindByClassCallback), ref endVals);
            return endVals.hWnd;
        }

        /// <summary>
        /// Find a window according to its Class Name, using a passed function
        /// </summary>
        /// <param name="testFunc">Function used to test the ClassName; takes ClassName in, returns True if match</param>
        /// <returns>The handle of a matching window, if found</returns>
        public static IntPtr FindByClassName(Func<string, bool> testFunc)
        {
            EnumObject endVals = new EnumObject();
            endVals.Data = testFunc;
            NativeMethods.EnumWindows(new EnumCallBack(FindByClassCallback), ref endVals);
            return endVals.hWnd;
        }

        /// <summary>
        /// Private function used to find a Child Window -- Callback for EnumChildWindows
        /// </summary>
        /// <param name="hWnd">The window currently being tested</param>
        /// <param name="param">EnumWindowsObject passed to EnumChildWindows</param>
        /// <returns>Whether to continue procesing windows or exit with this one</returns>
        private static bool FindChildCallback(IntPtr hWnd, ref EnumObject param)
        {
            if (param.Data is Func<IntPtr, bool>)
            {
                Func<IntPtr, bool> testFunc = (Func<IntPtr, bool>)param.Data;
                if (testFunc(hWnd))
                {
                    param.hWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Find a child window of the given window, using a passed function
        /// </summary>
        /// <param name="hwndParent">The parent window to find a child window of</param>
        /// <param name="testFunc">Function used to test the child window; takes the child window handle in, returns True if match</param>
        /// <returns>The handle of a matching window, if found</returns>
        public static IntPtr FindChild(IntPtr hwndParent, Func<IntPtr, bool> testFunc)
        {
            EnumObject endVals = new EnumObject();
            endVals.Data = testFunc;
            NativeMethods.EnumChildWindows(hwndParent, new EnumCallBack(FindChildCallback), ref endVals);
            return endVals.hWnd;
        }

        /// <summary>
        /// Retrieve a Process handle from any given window
        /// </summary>
        /// <param name="hWnd">The window handle of the desired process</param>
        /// <returns>Process object from the window handle passed</returns>
        public static System.Diagnostics.Process GetProcessFrom(IntPtr hWnd)
        {
            uint procId = 0;
            int activeWinThread = NativeMethods.GetWindowThreadProcessId(hWnd, out procId);
            if (procId == 0)
            {
                return null;
            }

            return System.Diagnostics.Process.GetProcessById((int)procId);
        }

        /// <summary>
        /// Set a windows position according to a Rectangle
        /// </summary>
        /// <param name="hWnd">The window Handle</param>
        /// <param name="coord">The new coordinates of the window</param>
        /// <returns>Whether the call was successful</returns>
        public static bool SetPosition(IntPtr hWnd, System.Drawing.Rectangle coord)
        {
            return NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, coord.X, coord.Y, coord.Width, coord.Height, 0);
        }

        /// <summary>
        /// Set a windows position according to a Rectangle, including possible flags
        /// </summary>
        /// <param name="hWnd">The window Handle</param>
        /// <param name="coord">The new coordinates of the window</param>
        /// <param name="flags">Flags to set the window position with; View SetWindowPos API for constants</param>
        /// <returns>Whether the call was successful</returns>
        public static bool SetPosition(IntPtr hWnd, System.Drawing.Rectangle coord, SetWindowsPositionFlags flags)
        {
            return NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, coord.X, coord.Y, coord.Width, coord.Height, flags);
        }

        /// <summary>
        /// Set a windows position according to a Rectangle, inserting it after a given window in the Z Order
        /// </summary>
        /// <param name="hWnd">The window Handle</param>
        /// <param name="coord">The new coordinates of the window</param>
        /// <param name="insertAfter">The Window to insert after; View SetWindowPos API for constants</param>
        /// <returns>Whether the call was successful</returns>
        public static bool SetPosition(IntPtr hWnd, System.Drawing.Rectangle coord, IntPtr insertAfter)
        {
            return NativeMethods.SetWindowPos(hWnd, insertAfter, coord.X, coord.Y, coord.Width, coord.Height, 0);
        }

        /// <summary>
        /// Set a windows position according to a Rectangle, including possible flags, inserting it after a given window in the Z Order
        /// </summary>
        /// <param name="hWnd">The window Handle</param>
        /// <param name="coord">The new coordinates of the window</param>
        /// <param name="insertAfter">The Window to insert after; View SetWindowPos API for constants</param>
        /// <param name="flags">Flags to set the window position with; View SetWindowPos API for constants</param>
        /// <returns>Whether the call was successful</returns>
        public static bool SetPosition(IntPtr hWnd, System.Drawing.Rectangle coord, IntPtr insertAfter, SetWindowsPositionFlags flags)
        {
            return NativeMethods.SetWindowPos(hWnd, insertAfter, coord.X, coord.Y, coord.Width, coord.Height, flags);
        }

        /// <summary>
        /// Determine whether a window is currently visible or not
        /// </summary>
        /// <param name="hWnd">Handle to the window to test for visibility</param>
        /// <returns>Boolean representing whether window is visible</returns>
        public static bool IsWindowVisible(IntPtr hWnd)
        {
            return NativeMethods.IsWindowVisible(hWnd);
        }
    }
}
