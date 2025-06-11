namespace RJCP.Core.Environment.Native
{
    using System;
    using System.Runtime.Versioning;
    using Microsoft.Win32;
    using Win32;
    using static Win32.Kernel32;

    [SupportedOSPlatform("windows")]
    internal class NativeWinVersion : INativeWinVersion
    {
        public void GetNativeSystemInfo(out SystemInfo systemInfo)
        {
            Kernel32.GetNativeSystemInfo(out SYSTEM_INFO lpSystemInfo);
            systemInfo = new() {
                dwOemId = lpSystemInfo.uProcessorInfo.dwOemId,
                wProcessorArchitecture = lpSystemInfo.uProcessorInfo.wProcessorArchitecture,
                dwPageSize = lpSystemInfo.dwPageSize,
                lpMinimumApplicationAddress = (ulong)lpSystemInfo.lpMinimumApplicationAddress.ToInt64(),
                lpMaximumApplicationAddress = (ulong)lpSystemInfo.lpMaximumApplicationAddress.ToInt64(),
                dwActiveProcessorMask = (ulong)lpSystemInfo.dwActiveProcessorMask.ToInt64(),
                dwNumberOfProcessors = lpSystemInfo.dwNumberOfProcessors,
                dwProcessorType = lpSystemInfo.dwProcessorType,
                dwAllocationGranularity = lpSystemInfo.dwAllocationGranularity,
                wProcessorLevel = lpSystemInfo.wProcessorLevel,
                wProcessorRevision = lpSystemInfo.wProcessorRevision
            };
        }

        public void GetSystemInfo(out SystemInfo systemInfo)
        {
            Kernel32.GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
            systemInfo = new() {
                dwOemId = lpSystemInfo.uProcessorInfo.dwOemId,
                wProcessorArchitecture = lpSystemInfo.uProcessorInfo.wProcessorArchitecture,
                dwPageSize = lpSystemInfo.dwPageSize,
                lpMinimumApplicationAddress = (ulong)lpSystemInfo.lpMinimumApplicationAddress.ToInt64(),
                lpMaximumApplicationAddress = (ulong)lpSystemInfo.lpMaximumApplicationAddress.ToInt64(),
                dwActiveProcessorMask = (ulong)lpSystemInfo.dwActiveProcessorMask.ToInt64(),
                dwNumberOfProcessors = lpSystemInfo.dwNumberOfProcessors,
                dwProcessorType = lpSystemInfo.dwProcessorType,
                dwAllocationGranularity = lpSystemInfo.dwAllocationGranularity,
                wProcessorLevel = lpSystemInfo.wProcessorLevel,
                wProcessorRevision = lpSystemInfo.wProcessorRevision
            };
        }

        public string BrandingFormatString(string format)
        {
            return WinBrand.BrandingFormatString(format);
        }

        public bool GetProductInfo(int osMajor, int osMinor, int spMajor, int spMinor, out uint productInfo)
        {
            return Kernel32.GetProductInfo(osMajor, osMinor, spMajor, spMinor, out productInfo);
        }

        public int GetSystemMetrics(int nIndex)
        {
            return Kernel32.GetSystemMetrics(nIndex);
        }

        public uint GetVersion()
        {
            return Kernel32.GetVersion();
        }

        public bool GetVersionEx(out OsVersionInfo osVersionInfo)
        {
            OSVERSIONINFO lpOsVersionInfo = new();
            bool result = Kernel32.GetVersionEx(lpOsVersionInfo);
            if (!result) {
                osVersionInfo = new();
                return false;
            }

            osVersionInfo = new() {
                MajorVersion = lpOsVersionInfo.MajorVersion,
                MinorVersion = lpOsVersionInfo.MinorVersion,
                BuildNumber = lpOsVersionInfo.BuildNumber,
                PlatformId = lpOsVersionInfo.PlatformId,
                CSDVersion = lpOsVersionInfo.CSDVersion
            };
            return true;
        }

        public bool GetVersionEx(out OsVersionInfoEx osVersionInfoEx)
        {
            OSVERSIONINFOEX lpOsVersionInfoEx = new();
            bool result = Kernel32.GetVersionEx(lpOsVersionInfoEx);
            if (!result) {
                osVersionInfoEx = new();
                return false;
            }

            osVersionInfoEx = new() {
                MajorVersion = lpOsVersionInfoEx.MajorVersion,
                MinorVersion = lpOsVersionInfoEx.MinorVersion,
                BuildNumber = lpOsVersionInfoEx.BuildNumber,
                PlatformId = lpOsVersionInfoEx.PlatformId,
                CSDVersion = lpOsVersionInfoEx.CSDVersion,
                ServicePackMajor = lpOsVersionInfoEx.ServicePackMajor,
                ServicePackMinor = lpOsVersionInfoEx.ServicePackMinor,
                SuiteMask = lpOsVersionInfoEx.SuiteMask,
                ProductType = lpOsVersionInfoEx.ProductType
            };
            return true;
        }

        public bool IsWow64Process(out bool wow64)
        {
            return Kernel32.IsWow64Process(Kernel32.GetCurrentProcess(), out wow64);
        }

        public bool IsWow64Process2(out ushort processMachine, out ushort nativeMachine)
        {
            return Kernel32.IsWow64Process2(Kernel32.GetCurrentProcess(), out processMachine, out nativeMachine);
        }

        public int RtlGetVersion(out OsVersionInfo osVersionInfo)
        {
            OSVERSIONINFO lpOsVersionInfo = new();
            int result = NtDll.RtlGetVersion(lpOsVersionInfo);
            if (result != 0) {
                osVersionInfo = new();
                return result;
            }

            osVersionInfo = new() {
                MajorVersion = lpOsVersionInfo.MajorVersion,
                MinorVersion = lpOsVersionInfo.MinorVersion,
                BuildNumber = lpOsVersionInfo.BuildNumber,
                PlatformId = lpOsVersionInfo.PlatformId,
                CSDVersion = lpOsVersionInfo.CSDVersion
            };
            return result;
        }

        public int RtlGetVersion(out OsVersionInfoEx osVersionInfoEx)
        {
            OSVERSIONINFOEX lpOsVersionInfoEx = new();
            int result = NtDll.RtlGetVersion(lpOsVersionInfoEx);
            if (result != 0) {
                osVersionInfoEx = new();
                return result;
            }

            osVersionInfoEx = new() {
                MajorVersion = lpOsVersionInfoEx.MajorVersion,
                MinorVersion = lpOsVersionInfoEx.MinorVersion,
                BuildNumber = lpOsVersionInfoEx.BuildNumber,
                PlatformId = lpOsVersionInfoEx.PlatformId,
                CSDVersion = lpOsVersionInfoEx.CSDVersion,
                ServicePackMajor = lpOsVersionInfoEx.ServicePackMajor,
                ServicePackMinor = lpOsVersionInfoEx.ServicePackMinor,
                SuiteMask = lpOsVersionInfoEx.SuiteMask,
                ProductType = lpOsVersionInfoEx.ProductType
            };
            return result;
        }

        public IRegistryKey OpenSubKey(string hive, string path)
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(hive);
            ThrowHelper.ThrowIfNullOrWhiteSpace(path);

            RegistryKey rk;
            if (string.Compare(hive, "HKLM", StringComparison.InvariantCultureIgnoreCase) == 0) {
                rk = Registry.LocalMachine.OpenSubKey(path);
            } else {
                throw new NotSupportedException("Only HKLM supported");
            }
            if (rk is null) return null;
            return new NativeRegistryKey(rk);
        }
    }
}
