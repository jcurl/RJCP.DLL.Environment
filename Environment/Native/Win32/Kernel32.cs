namespace RJCP.Core.Environment.Native.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal static partial class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetProductInfo(uint osMajor, uint osMinor, uint spMajor, uint spMinor, ref uint productInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSystemMetrics(uint nIndex);

        [DllImport("kernel32.dll")]
        public static extern uint GetVersion();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetVersionEx([In, Out] OSVERSIONINFO osVersionInfo);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetVersionEx([In, Out] OSVERSIONINFOEX osVersionInfoEx);

        [DllImport("kernel32.dll")]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool wow64);

        [DllImport("kernel32.dll")]
        public static extern bool IsWow64Process2(IntPtr hProcess, out ushort processMachine, out ushort nativeMachine);
    }
}
