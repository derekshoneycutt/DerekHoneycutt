using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Class used to handle Windows that should not receive activation upon user interaction
    /// </summary>
    public class NoActivateHandler
    {
        /// <summary>
        /// Gets the Handle to the window being handled
        /// </summary>
        public IntPtr WindowHandle { get; protected set; }

        private static uint m_UseOrStyle = (uint)WindowsStylesEx.NOACTIVATE;
        /// <summary>
        /// Gets the EXSTYLE Value that should be included via bitwise OR to make the window appropriately not activated upon interaction
        /// </summary>
        public static uint UserOrExStyle { get { return m_UseOrStyle; } }

        /// <summary>
        /// Initiate the handler with a given window
        /// </summary>
        /// <param name="hWnd">Handle to the window that will be treated for No Activation</param>
        public NoActivateHandler(IntPtr hWnd)
        {
            WindowHandle = hWnd;
        }

        /// <summary>
        /// Set the NOACTIVATE EXSTYLE on the Window
        /// </summary>
        public void SetStyle()
        {
            var exStyle = Windows.GetLong(WindowHandle, WindowsLongs.EXSTYLE);
            Windows.SetLong(WindowHandle, WindowsLongs.EXSTYLE, exStyle | UserOrExStyle);
        }

        /// <summary>
        /// Handle the WndProc procedure from Windows Forms
        /// </summary>
        /// <param name="m">Message as passed to the WndProc procedure</param>
        public void WinForms_WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessages.MOVING:
                    {
                        var newSize = new RECT();
                        newSize = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                        Windows.SetPosition(m.HWnd, (System.Drawing.Rectangle)newSize);
                    }
                    break;
                case (int)WindowsMessages.SIZING:
                    {
                        var newSize = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                        Windows.SetPosition(m.HWnd, (System.Drawing.Rectangle)newSize);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handle the WndProc procedure for all other purposes
        /// <para>WPF and other frameworks will most likely hook appropriately into this procedure</para>
        /// </summary>
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var m = new Message()
            {
                HWnd = hwnd,
                Msg = msg,
                WParam = wParam,
                LParam = lParam,
                Result = IntPtr.Zero
            };

            WinForms_WndProc(ref m);

            return m.Result;
        }
    }
}
