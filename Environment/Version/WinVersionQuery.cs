namespace RJCP.Core.Environment.Version
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Microsoft.Win32;
    using Native.Win32;

    /// <summary>
    /// Class to get information about the local machine.
    /// </summary>
    [CLSCompliant(false)]
    public class WinVersionQuery : WinVersion
    {
        private static WinVersionQuery s_Current = null;
        private bool m_NativeSystemInfo = false;
        private readonly bool m_IsNativeQuery = false;

        /// <summary>
        /// Default constructor, getting information about the local machine. Use the static method <c>LocalMachine</c>
        /// instead for efficiency.
        /// </summary>
        private WinVersionQuery()
        {
            m_IsNativeQuery = true;

#if NETFRAMEWORK
            // Create a security permission object to describe the UnmanagedCode permission:
            SecurityPermission perm =
               new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

            // Check that you have permission to access unmanaged code. If you don't have permission to access unmanaged
            // code, then this call will throw a SecurityException. Even though the CallUnmanagedCodeWithPermission
            // method is called from a stack frame that already calls Assert for unmanaged code, you still cannot call
            // native code. Because you use Deny here, the permission gets overwritten.
            try {
                perm.Assert();
            } catch {
                m_IsNativeQuery = false;
            }
#endif

            if (m_IsNativeQuery) {
                if (!GetVersionEx())
                    GetVersion();

                GetSystemInfo();
                DetectArchitecture();
                GetProductInfo();
                DetectWin2003R2();
                DetectWinXP();
                DetectWinXPx64();
            } else {
                GetNativeVersion();
            }
            Lock();
        }

        /// <summary>
        /// Get the version of the OS that we're running on.
        /// </summary>
        public static WinVersionQuery LocalMachine
        {
            get
            {
                if (s_Current is null) {
                    WinVersionQuery local = new WinVersionQuery();
                    s_Current = local;
                }

                return s_Current;
            }
        }

        /// <summary>
        /// Determine if we were able to obtain information using the Underlying Operating System. If it is native, you
        /// can trust all information.
        /// </summary>
        /// <remarks>
        /// There might be situations we can't obtain information natively. The biggest reason would be that we couldn't
        /// get permission to run unmanaged code, so we try to obtain as much information as possible via managed code.
        /// </remarks>
        public bool IsNativeQuery
        {
            get { return m_IsNativeQuery; }
        }

        /// <summary>
        /// Gets the Operating System version.
        /// </summary>
        /// <remarks>
        /// We use the Microsoft recommended way of getting the Operating System Version using GetVersionEx(), first
        /// with OSVERSIONINFO and then with OSVERSIONINFOEX if supported.
        /// </remarks>
        /// <returns><b>true</b> if we successfully retrieved OS version information.</returns>
        private bool GetVersionEx()
        {
            bool result;

            // Get the basic information. If this shows we've got a newer operating system, we can get more detailed
            // information later.
            Kernel32.OSVERSIONINFO info = new Kernel32.OSVERSIONINFO();
            try {
                result = Kernel32.GetVersionEx(info);
                if (!result) {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error,
                        "GetVersionEx() failure. Error = 0x" + error.ToString("8X"));
                }
            } catch {
                // The GetVersionEx() call doesn't exist, or it returned an error
                return false;
            }

            PlatformId = (WinPlatform)info.PlatformId;
            MajorVersion = info.MajorVersion;
            MinorVersion = info.MinorVersion;
            BuildNumber = info.BuildNumber;
            CSDVersion = info.CSDVersion;

            if (PlatformId == WinPlatform.WinNT) {
                if (MajorVersion < 4) {
                    // Windows 3.51 or earlier
                    return true;
                } else if (MajorVersion == 4) {
                    if (MinorVersion == 0) {
                        if (CSDVersion != "Service Pack 6") {
                            // Earlier than WinNT 4.0 SP6
                            return true;
                        }
                    }
                }
            } else if (PlatformId == WinPlatform.Win9x) {
                BuildNumber = (info.BuildNumber & 0xFFFF);
                return true;
            }

            Kernel32.OSVERSIONINFOEX infoex = new Kernel32.OSVERSIONINFOEX();
            result = Kernel32.GetVersionEx(infoex);
            if (!result) {
                int error = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(error,
                    "GetVersionEx() failure. Error = 0x" + error.ToString("8X"));
            }

            int ntstatus;
            Kernel32.OSVERSIONINFOEX rtlInfoEx = new Kernel32.OSVERSIONINFOEX();
            try {
                ntstatus = NtDll.RtlGetVersion(rtlInfoEx);
            } catch {
                // The RtlGetVersionEx() call doesn't exist, or it returned an error
                ntstatus = -1;
            }

            bool newer;
            if (ntstatus == 0) {
                // The direct call worked, and should overcome the API breakage that depends on a manifest. Just in case
                // that the real method returns a value that is older than the "real" windows API.
                Version vInfo = new Version(infoex.MajorVersion, infoex.MinorVersion, infoex.BuildNumber);
                Version vRtl = new Version(rtlInfoEx.MajorVersion, rtlInfoEx.MinorVersion, rtlInfoEx.BuildNumber);
                newer = vRtl >= vInfo;
            } else {
                newer = false;
            }

            if (newer) {
                PlatformId = (WinPlatform)rtlInfoEx.PlatformId;
                MajorVersion = rtlInfoEx.MajorVersion;
                MinorVersion = rtlInfoEx.MinorVersion;
                BuildNumber = rtlInfoEx.BuildNumber;
                CSDVersion = rtlInfoEx.CSDVersion;
                SuiteFlags = (WinSuite)rtlInfoEx.SuiteMask;
                ProductType = (WinProductType)rtlInfoEx.ProductType;
                ServicePackMajor = rtlInfoEx.ServicePackMajor;
                ServicePackMinor = rtlInfoEx.ServicePackMinor;
            } else {
                PlatformId = (WinPlatform)infoex.PlatformId;
                MajorVersion = infoex.MajorVersion;
                MinorVersion = infoex.MinorVersion;
                BuildNumber = infoex.BuildNumber;
                CSDVersion = infoex.CSDVersion;
                SuiteFlags = (WinSuite)infoex.SuiteMask;
                ProductType = (WinProductType)infoex.ProductType;
                ServicePackMajor = infoex.ServicePackMajor;
                ServicePackMinor = infoex.ServicePackMinor;
            }

            IsExtendedPropsSet = true;
            return true;
        }

        /// <summary>
        /// In case we couldn't get the version using the recommended way.
        /// </summary>
        /// <remarks>
        /// See http://msdn.microsoft.com/en-us/library/ms724439%28VS.85%29.aspx. This method doesn't support WinCE, as
        /// we don't get information about the platform ID. Win32s is determined by the upper bit in the system and then
        /// by the operating system version number.
        /// </remarks>
        /// <returns>If the function was successful.</returns>
        private void GetVersion()
        {
            uint version = Kernel32.GetVersion();

            MajorVersion = (int)(version & 0xFF);
            MinorVersion = (int)((version & 0xFF00) >> 8);

            if ((version & (1 << 31)) != 0) {
                // Win9x or Win32s
                BuildNumber = 0;
                if (MajorVersion == 3) {
                    PlatformId = WinPlatform.Win32s;
                } else {
                    PlatformId = WinPlatform.Win9x;
                }
            } else {
                // WinNT
                BuildNumber = (int)((version & 0x7FFF0000) >> 16);
                PlatformId = WinPlatform.WinNT;
            }
        }

        /// <summary>
        /// A method to get OS information natively from the .NET subsystem.
        /// </summary>
        /// <returns><c>true</c> if the information could be obtained; <c>false</c> otherwise.</returns>
        private void GetNativeVersion()
        {
            System.OperatingSystem os = System.Environment.OSVersion;

            switch (os.Platform) {
            case PlatformID.Win32S:
                PlatformId = WinPlatform.Win32s;
                break;
            case PlatformID.Win32Windows:
                PlatformId = WinPlatform.Win9x;
                break;
            case PlatformID.WinCE:
                PlatformId = WinPlatform.WinCE;
                break;
            case PlatformID.Win32NT:
                PlatformId = WinPlatform.WinNT;
                break;
            default:
                PlatformId = WinPlatform.Unknown;
                break;
            }

            if (PlatformId == WinPlatform.WinNT) {
                // Try to get the information from the registry
                ProductType = WinProductType.Unknown;
                try {
                    RegistryKey rk =
                        Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\ProductOptions");
                    if (rk != null) {
                        if (rk.GetValue("ProductType") is string producttype) {
                            if (producttype.Equals("WinNT")) ProductType = WinProductType.Workstation;
                            if (producttype.Equals("ServerNT")) ProductType = WinProductType.Server;
                        }
                    }
                } catch {
                    ProductType = WinProductType.Unknown;
                }

                // Found by reverse compiling System.Environment.OSVersion
                ServicePackMajor = os.Version.Revision >> 0x10;
                ServicePackMinor = os.Version.Revision & 0xFFFF;
            }
            MajorVersion = os.Version.Major;
            MinorVersion = os.Version.Minor;
            BuildNumber = os.Version.Build;
            CSDVersion = os.ServicePack;
            Architecture = WinArchitecture.Unknown;
        }

        /// <summary>
        /// Get the system info and set the processor architecture.
        /// </summary>
        private void GetSystemInfo()
        {
            Kernel32.SYSTEM_INFO lpSystemInfo = new Kernel32.SYSTEM_INFO();

            // GetNativeSystemInfo is independent if we're 64-bit or not But it needs _WIN32_WINNT 0x0501
            try {
                Kernel32.GetNativeSystemInfo(ref lpSystemInfo);
                Architecture = (WinArchitecture)lpSystemInfo.uProcessorInfo.wProcessorArchitecture;
                m_NativeSystemInfo = true;
            } catch {
                Architecture = WinArchitecture.Unknown;
                m_NativeSystemInfo = false;
            }

            if (Architecture == WinArchitecture.Unknown || !m_NativeSystemInfo) {
                try {
                    Kernel32.GetSystemInfo(ref lpSystemInfo);
                    Architecture = (WinArchitecture)lpSystemInfo.uProcessorInfo.wProcessorArchitecture;
                } catch {
                    Architecture = WinArchitecture.Unknown;
                }
            }
        }

        /// <summary>
        /// Get product information that is available in Windows Vista and later.
        /// </summary>
        private void GetProductInfo()
        {
            if (MajorVersion < 0 || MinorVersion < 0) {
                // Don't have service pack information
                throw new InvalidOperationException("Internal Error: Must get OS version information first");
            }

            if (!IsExtendedPropsSet) return;

            uint productInfo = 0;
            bool result;
            try {
                result = Kernel32.GetProductInfo((uint)MajorVersion, (uint)MinorVersion,
                    (uint)ServicePackMajor, (uint)ServicePackMinor, ref productInfo);
            } catch {
                // The operating system doesn't support this function call
                result = false;
            }
            if (!result) productInfo = 0;

            ProductInfo = (WinProductInfo)productInfo;
        }

        private void DetectWin2003R2()
        {
            if (MajorVersion == Win2003.MajorVersion && MinorVersion == Win2003.MinorVersion) {
                ServerR2 = Kernel32.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_SERVERR2) != 0;
            }
        }

        private void DetectWinXP()
        {
            int result;

            if (MajorVersion == WinXP.MajorVersion && MinorVersion == WinXP.MinorVersion) {
                ProductInfo = WinProductInfo.Undefined;

                result = Kernel32.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_MEDIACENTER);
                if (result != 0) ProductInfo = WinProductInfo.MediaCenter;

                result = Kernel32.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_TABLETPC);
                if (result != 0) ProductInfo = WinProductInfo.TabletPc;

                result = Kernel32.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_STARTER);
                if (result != 0) ProductInfo = WinProductInfo.Starter;

                if (ProductInfo == WinProductInfo.Undefined) {
                    if (SuiteFlag(WinSuite.Personal)) {
                        ProductInfo = WinProductInfo.Home_Premium;
                    } else {
                        ProductInfo = WinProductInfo.Professional;
                    }
                }
            }
        }

        private void DetectWinXPx64()
        {
            if (MajorVersion == WinXPx64.MajorVersion && MinorVersion == WinXPx64.MinorVersion) {
                if (ProductType == WinProductType.Workstation) {
                    ProductInfo = WinProductInfo.Professional;
                }
            }
        }

        private void DetectArchitecture()
        {
            // We try to determine if we're a WOW64 process if we don't know the architecture or if we're x86 and
            // NativeSystemInfo didn't work.
            bool wow64 = false;
            bool result;
            try {
                result = Kernel32.IsWow64Process(Kernel32.GetCurrentProcess(), ref wow64);
            } catch {
                result = false;
            }

            if (result) {
                if (wow64) {
                    // wow64 == true: 32-bit process on 64-bit windows.
                    Architecture = WinArchitecture.x86_x64;
                } else {
                    // wow64 == false: 32-bit on 32-bit; or 64-bit on 64-bit
                }
            }
        }
    }
}
