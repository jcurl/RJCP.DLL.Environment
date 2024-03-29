﻿namespace RJCP.Core.Environment.Native.Win32
{
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    [SupportedOSPlatform("windows")]
    internal static class WinBrand
    {
        [DllImport("winbrand.dll", CharSet = CharSet.Unicode)]
#if NET45_OR_GREATER || NET6_0_OR_GREATER
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
#endif
        public static extern string BrandingFormatString(string format);
    }
}
