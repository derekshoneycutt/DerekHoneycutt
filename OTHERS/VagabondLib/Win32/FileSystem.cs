using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Functions to handle Win32 API Calls for File System operations
    /// </summary>
    public static class FileSystem
    {
        private static class NativeMethods
        {
            /// <summary>
            /// Retrieves the name of the executable file for the specified process.
            /// </summary>
            /// <param name="hProcess">A handle to the process. The handle must have the PROCESS_QUERY_INFORMATION or PROCESS_QUERY_LIMITED_INFORMATION access right.</param>
            /// <param name="lpImageFileName">A pointer to a buffer that receives the full path to the executable file.</param>
            /// <param name="nSize">The size of the lpImageFileName buffer, in characters.</param>
            /// <returns>The length of the string copied to the buffer.</returns>
            [DllImport("psapi.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern uint GetProcessImageFileName(
                IntPtr hProcess,
                [Out] 
                [MarshalAs(UnmanagedType.LPStr)]
                StringBuilder lpImageFileName,
                [In] 
                [MarshalAs(UnmanagedType.U4)] 
                int nSize);

            /// <summary>
            /// Retrieves information about MS-DOS device names. 
            /// The function can obtain the current mapping for a particular MS-DOS device name. 
            /// The function can also obtain a list of all existing MS-DOS device names.
            /// </summary>
            /// <param name="lpDeviceName">An MS-DOS device name string specifying the target of the query. 
            ///     The device name cannot have a trailing backslash; for example, use "C:", not "C:\".</param>
            /// <param name="lpTargetPath">
            /// A pointer to a buffer that will receive the result of the query. The function fills this buffer with one or more null-terminated strings. 
            /// The final null-terminated string is followed by an additional NULL.
            /// </param>
            /// <param name="ucchMax">The maximum number of TCHARs that can be stored into the buffer pointed to by lpTargetPath.</param>
            /// <returns>If the function succeeds, the return value is the number of TCHARs stored into the buffer pointed to by lpTargetPath.</returns>
            [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern uint QueryDosDevice(
                [MarshalAs(UnmanagedType.LPStr)]
                string lpDeviceName,
                [MarshalAs(UnmanagedType.LPStr)]
                StringBuilder lpTargetPath,
                int ucchMax);

            [DllImport("kernel32.dll")]
            public static extern bool QueryFullProcessImageName(IntPtr hprocess, int dwFlags,
                           StringBuilder lpExeName, out int size);

            [Flags]
            public enum ProcessAccessFlags : uint
            {
                All = 0x001F0FFF,
                Terminate = 0x00000001,
                CreateThread = 0x00000002,
                VirtualMemoryOperation = 0x00000008,
                VirtualMemoryRead = 0x00000010,
                VirtualMemoryWrite = 0x00000020,
                DuplicateHandle = 0x00000040,
                CreateProcess = 0x000000080,
                SetQuota = 0x00000100,
                SetInformation = 0x00000200,
                QueryInformation = 0x00000400,
                QueryLimitedInformation = 0x00001000,
                Synchronize = 0x00100000
            }

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
                           bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool CloseHandle(IntPtr hHandle);
        }




        /// <summary>
        /// Map a drive letter to a drive path
        /// </summary>
        /// <param name="letterDrive">The letter of the drive to map</param>
        /// <returns>The drive path, if it exists</returns>
        public static string MapLetterToDrive(char letterDrive)
        {
            StringBuilder driveTest = new StringBuilder(1024);
            NativeMethods.QueryDosDevice(new string(new char[] { letterDrive, ':' }), driveTest, driveTest.Capacity);
            return driveTest.ToString();
        }

        /// <summary>
        /// Get the filename of a running process's executable
        /// </summary>
        /// <param name="theProc">The process to get the filename of</param>
        /// <returns>The Filename of the executable</returns>
        public static string GetFileNameOfProcess(System.Diagnostics.Process theProc)
        {
            StringBuilder procFileName = new StringBuilder(1024);
            string exeFile = String.Empty;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                IntPtr hprocess = NativeMethods.OpenProcess(NativeMethods.ProcessAccessFlags.QueryLimitedInformation,
                                              false, theProc.Id);
                if (hprocess != IntPtr.Zero)
                {
                    try
                    {
                        int size = procFileName.Capacity;
                        if (NativeMethods.QueryFullProcessImageName(hprocess, 0, procFileName, out size))
                        {
                            return procFileName.ToString();
                        }
                    }
                    finally
                    {
                        NativeMethods.CloseHandle(hprocess);
                    }
                }
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            else
            {
                NativeMethods.GetProcessImageFileName(theProc.Handle, procFileName, procFileName.Capacity);
                exeFile = procFileName.ToString();

                //We have a drive path, but it will not work as a real filename, so deduce to get it right
                string tempStr = String.Empty;
                string replaceStr = String.Empty;
                if (exeFile.StartsWith("\\Device\\Mup\\", StringComparison.OrdinalIgnoreCase))
                { // UNC paths aren't covered so well by the Map Letter To Drive, but they just need the "\Device\Mup" replaced with an extra "\"
                    tempStr = "\\Device\\Mup";
                    replaceStr = "\\";
                }
                else
                {
                    for (char driveOn = 'A'; driveOn != 'Z'; ++driveOn)
                    {
                        tempStr = MapLetterToDrive(driveOn);
                        if (!String.IsNullOrEmpty(tempStr))
                        {
                            if (tempStr.Length < exeFile.Length)
                            {
                                if (tempStr.Equals(exeFile.Substring(0, tempStr.Length), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    replaceStr = new string(new char[] { driveOn, ':' });
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!String.IsNullOrEmpty(replaceStr))
                {
                    exeFile = exeFile.Remove(0, tempStr.Length);
                    exeFile = exeFile.Insert(0, replaceStr);
                }
                return exeFile;
            }
        }
    }
}
