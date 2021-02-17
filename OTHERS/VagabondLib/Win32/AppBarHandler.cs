using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VagabondLib.Win32.Internal.AppBar;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Class used to handle Win32 AppBars and the Windows that consume them
    /// </summary>
    public class AppBarHandler
    {
        private static readonly Dictionary<ScreenEdge, Func<IEdgeHandler>> EdgeHandlerFactory =
            new Dictionary<ScreenEdge, Func<IEdgeHandler>>()
            {
                {ScreenEdge.Top, () => new TopEdgeHandler()},
                {ScreenEdge.Right, () => new RightEdgeHandler()},
                {ScreenEdge.Bottom, () => new BottomEdgeHandler()},
                {ScreenEdge.Left, () => new LeftEdgeHandler()}
            };

        private static readonly Dictionary<WindowsMessages, Action<AppBarHandler, Message>> WinMsgs =
            new Dictionary<WindowsMessages, Action<AppBarHandler, Message>>()
            {
                { WindowsMessages.DESTROY, (f, m) => f.OnDestroy(m) },
                { WindowsMessages.ACTIVATE, (f, m) => f.OnActivate(m) },
                { WindowsMessages.WINDOWPOSCHANGED, (f, m) => f.OnWindowPosChanged(m) },
                { WindowsMessages.MOVING, (f, m) => f.OnMoving(m) },
                { WindowsMessages.SIZING, (f, m) => f.OnSizing(m) },
                { WindowsMessages.USER, (f, m) => f.OnUserMessage(m) }
            };

        private AppBar appbar;
        private System.Drawing.Rectangle appbarRect;
        private IEdgeHandler edgeHandler;

        /// <summary>
        /// Gets whether the AppBar is currently active and being displayed as an AppBar
        /// </summary>
        public bool AppBarIsActive { get; private set; }
        /// <summary>
        /// Gets the current Side of the screen the AppBar is consuming
        /// </summary>
        public ScreenEdge AppBarCurrentLocation
        {
            get
            {
                if (edgeHandler == null)
                {
                    return ScreenEdge.None;
                }
                else
                {
                    return edgeHandler.ThisEdge;
                }
            }
        }
        /// <summary>
        /// Gets the variable dimension of the AppBar; May be Height or Width, depending on Location
        /// </summary>
        public int AppBarDimension { get; private set; }
        /// <summary>
        /// Gets or Sets whether the AppBar will remain on the Primary Monitor only
        /// </summary>
        public bool UsePrimaryMonitorOnly { get; set; }
        /// <summary>
        /// Gets the current Size and Location of the AppBar on the screen in literal numbers
        /// </summary>
        public System.Drawing.Rectangle AppBarBounds { get { return appbarRect; } }

        /// <summary>
        /// Check the size of the AppBar and ensure that it will be displayed properly on one screen
        /// </summary>
        /// <param name="dimension">Size of the AppBar in its variable dimension; may be Height or Width depending on location</param>
        private void CheckDockSize(int dimension)
        {
            if (UsePrimaryMonitorOnly)
            {
                appbarRect = edgeHandler.GetAppBarRectFromScreenRect(Screen.PrimaryScreen.Bounds, dimension);
            }
            else
            {
                appbarRect = edgeHandler.GetAppBarRectFromScreenRect(SystemInformation.VirtualScreen, dimension);
            }

            //Correct the Appbar to the Proper Screen
            appbarRect = edgeHandler.FixChangedRect(appbarRect, dimension, appbar);
        }

        /// <summary>
        /// Set the size of the AppBar to the current values and tell Windows appropriately how to display the AppBar
        /// </summary>
        private void SizeToNewValues()
        {
            appbarRect = appbar.QueryPos(AppBarCurrentLocation, appbarRect);
            appbarRect = appbar.SetPos(AppBarCurrentLocation, appbarRect);

            Windows.SetPosition(appbar.hWnd, appbarRect, Windows.HWND_TOPMOST,
                                            SetWindowsPositionFlags.NOACTIVATE);

            AppBarDimension = edgeHandler.GetCurrentDimension(appbarRect);
        }


        /// <summary>
        /// Initialize an AppBarHandler for a specific window
        /// </summary>
        /// <param name="winHandle">Handler of the window to hook into</param>
        public AppBarHandler(IntPtr winHandle)
        {
            edgeHandler = null;
            appbar = new AppBar(winHandle);
            AppBarIsActive = false;
            UsePrimaryMonitorOnly = false;
        }

        /// <summary>
        /// Activate the AppBar at the given location on the screen
        /// </summary>
        /// <param name="newLocat">Location of the AppBar</param>
        /// <returns>True if AppBar was successfully created</returns>
        public bool ActivateAppBar(ScreenEdge newLocat)
        {
            DeactivateAppBar();

            Func<IEdgeHandler> edgeHandlerFact;
            if (EdgeHandlerFactory.TryGetValue(newLocat, out edgeHandlerFact))
            {
                edgeHandler = edgeHandlerFact();
            }
            else
            {
                return false;
            }

            var winBounds = Windows.GetRectangle(appbar.hWnd);
            AppBarDimension = edgeHandler.GetCurrentDimension(winBounds);

            appbar = new AppBar(appbar.hWnd);
            if (!appbar.CreateNew())
            {
                return false;
            }
            CheckDockSize(AppBarDimension);
            SizeToNewValues();

            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            AppBarIsActive = true;
            return true;
        }

        /// <summary>
        /// Deactivate the AppBar if it is currently active
        /// </summary>
        public void DeactivateAppBar()
        {
            if (AppBarIsActive)
            {
                SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;

                AppBarIsActive = false;
                appbar.Remove();
                edgeHandler = null;
            }
        }

        /// <summary>
        /// Handling of WndProc for WinForms Windows
        /// </summary>
        public void WinForms_WndProc(ref Message m)
        {
            Action<AppBarHandler, Message> RunForMessage;
            if (WinMsgs.TryGetValue((WindowsMessages)m.Msg, out RunForMessage))
            {
                if (RunForMessage != null)
                {
                    RunForMessage(this, m);
                }
            }
        }

        /// <summary>
        /// Handling of WndProc for all non-WinForms Windows
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

        /*
         * The following are events specifically from the WndProc event above.
         * These handle the messages that Windows sends to the AppBar window
         * and acts appropriately, destroying, creating, resizing or moving the AppBar as needed
         */
        private void OnDestroy(Message m)
        {
            DeactivateAppBar();
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (AppBarIsActive)
            {
                appbar.Remove();
                if (!appbar.CreateNew())
                {
                    AppBarIsActive = false;
                    return;
                }
                CheckDockSize(AppBarDimension);
                SizeToNewValues();
            }
        }

        private void OnWindowPosChanged(Message m)
        {
            if (AppBarIsActive)
            {
                appbar.NotifyPosChange();
            }
        }

        private void OnMoving(Message m)
        {
            if (AppBarIsActive)
            {
                var newSize = new RECT();
                newSize = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                newSize.Left = appbarRect.Left;
                newSize.Right = appbarRect.Right;
                newSize.Top = appbarRect.Top;
                newSize.Bottom = appbarRect.Bottom;
                Marshal.StructureToPtr(newSize, m.LParam, false);
            }
        }

        private void OnSizing(Message m)
        {
            if (AppBarIsActive)
            {
                var newSize = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                var origBounds = Windows.GetRectangle(m.HWnd);
                newSize = edgeHandler.FixResizing(origBounds, newSize);
                AppBarDimension = edgeHandler.GetCurrentDimension(newSize);
                Marshal.StructureToPtr((RECT)appbarRect, m.LParam, false);
                Windows.SetPosition(m.HWnd, (System.Drawing.Rectangle)newSize);
            }
        }

        private void OnUserMessage(Message m)
        {
            if (AppBarIsActive)
            {
                switch ((AppBar.Notify)m.WParam)
                {
                    case AppBar.Notify.ABN_POSCHANGED:
                        appbarRect = edgeHandler.FixChangedRect(appbarRect, AppBarDimension, appbar);
                        SizeToNewValues();
                        break;
                    case AppBar.Notify.ABN_FULLSCREENAPP:
                        if (m.LParam == IntPtr.Zero)
                        {
                            Windows.SetPosition(m.HWnd, new System.Drawing.Rectangle(0, 0, 0, 0), Windows.HWND_TOPMOST,
                                SetWindowsPositionFlags.NOMOVE | SetWindowsPositionFlags.NOSIZE | SetWindowsPositionFlags.NOACTIVATE);
                        }
                        else
                        {
                            Windows.SetPosition(m.HWnd, new System.Drawing.Rectangle(0, 0, 0, 0), Windows.HWND_BOTTOM,
                                SetWindowsPositionFlags.NOMOVE | SetWindowsPositionFlags.NOSIZE | SetWindowsPositionFlags.NOACTIVATE);
                        }
                        break;
                    case AppBar.Notify.ABN_WINDOWARRANGE:
                    case AppBar.Notify.ABN_STATECHANGE:
                    default:
                        break;
                }
            }
        }

        private void OnActivate(Message m)
        {
            if (AppBarIsActive)
            {
                appbar.Activate();
            }
        }

        /// <summary>
        /// Event to call when Resizing has ended, and all resizing should be completed
        /// </summary>
        public void OnResizeEnd(EventArgs e)
        {
            if (AppBarIsActive)
            {
                appbarRect = edgeHandler.FixResizeEnd(appbarRect, AppBarDimension);

                SizeToNewValues();
            }
        }
    }
}
