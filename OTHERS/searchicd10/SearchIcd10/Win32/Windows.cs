using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SearchIcd10.Win32
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

        private static class NativeMethods
        {
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
            public static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

            /// <summary>
            /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
            /// </summary>
            /// <param name="lpString">The message to be registered.</param>
            /// <returns>A message identifier in the range 0xC000 through 0xFFFF.</returns>
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern int RegisterWindowMessage([MarshalAs(UnmanagedType.LPStr)] string lpString);

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
        /// Send the EM_REPLACESEL to a specific Window's Control, with a given text to insert
        /// </summary>
        /// <param name="hWndControl">Control to send the message</param>
        /// <param name="newText">Text intended to insert into the control</param>
        public static void SendReplaceSelection(IntPtr hWndControl, string newText)
        {
            NativeMethods.SendMessage(hWndControl, WindowsMessages.EM_REPLACESEL, (IntPtr)1, newText);
        }
    }
}
