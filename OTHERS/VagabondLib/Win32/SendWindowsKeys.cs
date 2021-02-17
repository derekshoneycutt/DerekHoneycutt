using System;
using System.Runtime.InteropServices;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Class used to more effectively utilize SendKeys than what the .NET's SendKeys can do
    /// </summary>
    public static class SendWindowsKeys
    {
        // Follows is a bunch of stuff from pinvoke ... some simplified, because we don't need so much fluff right now... this is kind of a quick hack
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            int dx;
            int dy;
            uint mouseData;
            uint dwFlags;
            uint time;
            IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            uint uMsg;
            ushort wParamL;
            ushort wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            internal uint type;
            internal InputUnion U;
            internal static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern uint MapVirtualKey(uint uCode, uint uMapType);

            [DllImport("User32.dll")]
            public static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);
        }

        private const int INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 2;

        /// <summary>
        /// Simulate a key press with a simulated modifier key pressed
        /// </summary>
        /// <param name="modKey">Modifier key to hold down for the key press</param>
        /// <param name="pressKey">Key to simulate the press of</param>
        public static void KeyPressModified(uint modKey, uint pressKey)
        {
            INPUT[] inputs = new INPUT[] {
                new INPUT {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion {
                        ki = new KEYBDINPUT {
                            wVk = (ushort)modKey,
                            wScan = (ushort)NativeMethods.MapVirtualKey(modKey, 0)
                        }
                    }
                },
                new INPUT {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion {
                        ki = new KEYBDINPUT {
                            wVk = (ushort)pressKey,
                            wScan = (ushort)NativeMethods.MapVirtualKey(pressKey, 0)
                        }
                    }
                },
                new INPUT {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion {
                        ki = new KEYBDINPUT {
                            wVk = (ushort)pressKey,
                            wScan = (ushort)NativeMethods.MapVirtualKey(pressKey, 0),
                            dwFlags = KEYEVENTF_KEYUP
                        }
                    }
                },
                new INPUT {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion {
                        ki = new KEYBDINPUT {
                            wVk = (ushort)modKey,
                            wScan = (ushort)NativeMethods.MapVirtualKey(modKey, 0),
                            dwFlags = KEYEVENTF_KEYUP
                        }
                    }
                }
            };

            uint intReturn = NativeMethods.SendInput(4, inputs, Marshal.SizeOf(typeof(INPUT)));
            if (intReturn == 0)
            {
                throw new Exception(String.Format("Could not send modfied key: {0} + {1}", modKey, pressKey));
            }
        }
    }
}
