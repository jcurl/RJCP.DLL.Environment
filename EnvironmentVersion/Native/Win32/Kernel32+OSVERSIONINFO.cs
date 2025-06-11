namespace RJCP.Core.Environment.Native.Win32
{
    using System.Runtime.InteropServices;

    internal static partial class Kernel32
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OSVERSIONINFO
        {
            public uint OSVersionInfoSize;
            public uint MajorVersion;
            public uint MinorVersion;
            public uint BuildNumber;
            public uint PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
            public string CSDVersion;

            public OSVERSIONINFO()
            {
                OSVersionInfoSize = unchecked((uint)(Marshal.SizeOf(this)));
            }
        }
    }
}
