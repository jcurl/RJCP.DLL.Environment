namespace RJCP.Core.Environment.Version
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;
    using System.Xml;
    using Native;
    using Native.Win32;
    using Resources;

    /// <summary>
    /// Class to get information about the local machine.
    /// </summary>
    internal class WinVersionQuery : WinVersion
    {
        private readonly INativeWinVersion m_WinVersion;

        /// <summary>
        /// Default constructor, getting information about the local machine. Use the static method <c>LocalMachine</c>
        /// instead for efficiency.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Windows Version Query is only supported on Windows.</exception>
        [SupportedOSPlatform("windows")]
        public WinVersionQuery()
        {
            if (!Platform.IsWinNT())
                throw new PlatformNotSupportedException(Messages.PlatformNotSupportedEx);

            m_WinVersion = WinVersionFactory.Create();
            WinVersionQueryInternal();
        }

        public WinVersionQuery(string fileName)
        {
            m_WinVersion = WinVersionFactory.Create(fileName);
            WinVersionQueryInternal();
        }

        public WinVersionQuery(XmlDocument winDoc)
        {
            m_WinVersion = WinVersionFactory.Create(winDoc);
            WinVersionQueryInternal();
        }

        public WinVersionQuery(XmlDocumentFragment winDocFragment)
        {
            m_WinVersion = WinVersionFactory.Create(winDocFragment);
            WinVersionQueryInternal();
        }

        public WinVersionQuery(XmlNode winDocNode)
        {
            m_WinVersion = WinVersionFactory.Create(winDocNode);
            WinVersionQueryInternal();
        }

        private void WinVersionQueryInternal()
        {
            if (!GetVersionEx())
                GetVersion();

            DetectArchitecture();
            GetProductInfo();
            DetectWin2003R2();
            DetectWinXP();
            DetectWinXPx64();
            DetectWin10();
            Lock();
        }

        /// <summary>
        /// Gets the Operating System version.
        /// </summary>
        /// <remarks>
        /// We use the Microsoft recommended way of getting the Operating System Version using GetVersionEx(), first
        /// with OSVERSIONINFO and then with OSVERSIONINFOEX if supported.
        /// </remarks>
        /// <returns><see langword="true"/> if we successfully retrieved OS version information.</returns>
        private bool GetVersionEx()
        {
            bool result;

            // Get the basic information. If this shows we've got a newer operating system, we can get more detailed
            // information later.
            OsVersionInfo info;
            try {
                result = m_WinVersion.GetVersionEx(out info);
                if (!result) {
                    int error = Marshal.GetLastWin32Error();
                    string message = string.Format(Messages.Win32Ex_GetVersionEx, error);
                    throw new Win32Exception(error, message);
                }
            } catch {
                // The GetVersionEx() call doesn't exist, or it returned an error
                return false;
            }

            PlatformId = (WinPlatform)info.PlatformId;
            MajorVersion = unchecked((int)(info.MajorVersion));
            MinorVersion = unchecked((int)(info.MinorVersion));
            BuildNumber = unchecked((int)(info.BuildNumber));
            CSDVersion = info.CSDVersion;

            if (PlatformId != WinPlatform.WinNT) {
                BuildNumber = unchecked((int)(info.BuildNumber & 0xFFFF));

                // Even though there might be a version of OSVERSIONINFOEX, we don't use it, as it only contains service
                // pack information, which isn't used (the CSDVersion is used on Win9x).
                return true;
            }

            // Extended information isn't available.
            result = m_WinVersion.GetVersionEx(out OsVersionInfoEx infoex);
            if (!result) {
                if (MajorVersion == 4 && MinorVersion == 0) {
                    if (CSDVersion.StartsWith("Service Pack ", StringComparison.InvariantCultureIgnoreCase)) {
                        if (uint.TryParse(CSDVersion.Substring(13), out uint spmajor) && spmajor < 6) {
                            ServicePackMajor = unchecked((int)spmajor);
                            ServicePackMinor = 0;
                        }
                    }
                }
                return true;
            }

            int ntstatus;
            OsVersionInfoEx rtlInfoEx;
            try {
                ntstatus = m_WinVersion.RtlGetVersion(out rtlInfoEx);
            } catch {
                // Needed because the compiler thinks it might be undefined otherwise.
                rtlInfoEx = new();

                // The RtlGetVersionEx() call doesn't exist, or it returned an error
                ntstatus = -1;
            }

            bool newer;
            if (ntstatus == 0) {
                // The direct call worked, and should overcome the API breakage that depends on a manifest. Just in case
                // that the real method returns a value that is older than the "real" windows API.
                Version vInfo = new(
                    unchecked((int)(info.MajorVersion)),
                    unchecked((int)(info.MinorVersion)),
                    unchecked((int)(info.BuildNumber)));
                Version vRtl = new(
                    unchecked((int)(rtlInfoEx.MajorVersion)),
                    unchecked((int)(rtlInfoEx.MinorVersion)),
                    unchecked((int)(rtlInfoEx.BuildNumber)));
                newer = vRtl >= vInfo;
            } else {
                newer = false;
            }

            if (newer) {
                PlatformId = (WinPlatform)rtlInfoEx.PlatformId;
                MajorVersion = unchecked((int)(rtlInfoEx.MajorVersion));
                MinorVersion = unchecked((int)(rtlInfoEx.MinorVersion));
                BuildNumber = unchecked((int)(rtlInfoEx.BuildNumber));
                if (string.IsNullOrEmpty(rtlInfoEx.CSDVersion)) {
                    CSDVersion = infoex.CSDVersion;
                } else {
                    CSDVersion = rtlInfoEx.CSDVersion;
                }
                SuiteFlags = (WinSuite)rtlInfoEx.SuiteMask;
                ProductType = (WinProductType)rtlInfoEx.ProductType;
                ServicePackMajor = rtlInfoEx.ServicePackMajor;
                ServicePackMinor = rtlInfoEx.ServicePackMinor;
            } else {
                PlatformId = (WinPlatform)infoex.PlatformId;
                MajorVersion = unchecked((int)(infoex.MajorVersion));
                MinorVersion = unchecked((int)(info.MinorVersion));
                BuildNumber = unchecked((int)(info.BuildNumber));
                CSDVersion = infoex.CSDVersion;
                SuiteFlags = (WinSuite)infoex.SuiteMask;
                ProductType = (WinProductType)infoex.ProductType;
                ServicePackMajor = infoex.ServicePackMajor;
                ServicePackMinor = infoex.ServicePackMinor;
            }

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
        private void GetVersion()
        {
            uint version = m_WinVersion.GetVersion();

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

        private void DetectArchitecture()
        {
            if (DetectArchitectureWithWow2()) return;
            DetectArchitectureWithSystemInfo();
        }

        private bool DetectArchitectureWithWow2()
        {
            try {
                bool result = m_WinVersion.IsWow64Process2(out ushort processMachine, out ushort nativeMachine);
                if (!result) return false;

                NativeArchitecture = FromImageFileMachine(nativeMachine);
                if (processMachine == Kernel32.IMAGE_FILE_MACHINE.UNKNOWN) {
                    // This is not a WoW process, so it's the same as the native architecture.
                    Architecture = NativeArchitecture;
                } else {
                    Architecture = FromImageFileMachine(processMachine);
                }
                return true;
            } catch (EntryPointNotFoundException) {
                return false;
            }
        }

        private void DetectArchitectureWithSystemInfo()
        {
            SystemInfo systemInfo;

            // GetNativeSystemInfo is independent if we're 64-bit or not But it needs _WIN32_WINNT 0x0501
            ushort processorNativeArchitecture;
            try {
                m_WinVersion.GetNativeSystemInfo(out systemInfo);
                processorNativeArchitecture = systemInfo.wProcessorArchitecture;
            } catch (EntryPointNotFoundException) {
                processorNativeArchitecture = Kernel32.PROCESSOR_ARCHITECTURE.UNKNOWN;
            }

            if (processorNativeArchitecture == Kernel32.PROCESSOR_ARCHITECTURE.UNKNOWN) {
                m_WinVersion.GetSystemInfo(out systemInfo);
                processorNativeArchitecture = systemInfo.wProcessorArchitecture;
            }

            NativeArchitecture = FromProcessorArchitecture(processorNativeArchitecture);

            switch (NativeArchitecture) {
            case WinArchitecture.IA64:
            case WinArchitecture.x64:
                bool result = m_WinVersion.IsWow64Process(out bool wow64);
                if (result && wow64) {
                    Architecture = WinArchitecture.x86;
                }
                break;
            }

            if (Architecture == WinArchitecture.Unknown)
                Architecture = NativeArchitecture;
        }

        /// <summary>
        /// Get product information that is available in Windows Vista and later.
        /// </summary>
        private void GetProductInfo()
        {
            if (MajorVersion < 0 || MinorVersion < 0) {
                // Don't have service pack information
                throw new InvalidOperationException(Messages.InvalidOperationEx_InternalGetOsVersion);
            }

            uint productInfo = 0;
            bool result;
            try {
                result = m_WinVersion.GetProductInfo(MajorVersion, MinorVersion,
                    ServicePackMajor, ServicePackMinor, out productInfo);
            } catch {
                // The operating system doesn't support this function call
                result = false;
            }
            if (!result) return;

            ProductInfo = (WinProductInfo)productInfo;
        }

        private void DetectWin2003R2()
        {
            if (MajorVersion == Win2003.MajorVersion && MinorVersion == Win2003.MinorVersion) {
                ServerR2 = m_WinVersion.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_SERVERR2) != 0;
            }
        }

        private void DetectWinXP()
        {
            int result;

            if (MajorVersion == WinXP.MajorVersion && MinorVersion == WinXP.MinorVersion) {
                ProductInfo = WinProductInfo.Undefined;

                result = m_WinVersion.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_MEDIACENTER);
                if (result != 0) ProductInfo = WinProductInfo.MediaCenter;

                result = m_WinVersion.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_TABLETPC);
                if (result != 0) ProductInfo = WinProductInfo.TabletPc;

                result = m_WinVersion.GetSystemMetrics(Kernel32.SYSTEM_METRICS.SM_STARTER);
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

        private static WinArchitecture FromImageFileMachine(ushort imageFileMachine)
        {
            switch (imageFileMachine) {
            case Kernel32.IMAGE_FILE_MACHINE.UNKNOWN: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.TARGET_HOST: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.I386: return WinArchitecture.x86;
            case Kernel32.IMAGE_FILE_MACHINE.MIPS_R3000: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.MIPS_R3000_BE: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.MIPS_R4000: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.MIPS_R10000: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.MIPS_WCEMIPSV2: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.ALPHA: return WinArchitecture.Alpha;
            case Kernel32.IMAGE_FILE_MACHINE.ALPHA64: return WinArchitecture.Alpha64;
            case Kernel32.IMAGE_FILE_MACHINE.SH3: return WinArchitecture.SHX;
            case Kernel32.IMAGE_FILE_MACHINE.SH3DSP: return WinArchitecture.SHX;
            case Kernel32.IMAGE_FILE_MACHINE.SH3E: return WinArchitecture.SHX;
            case Kernel32.IMAGE_FILE_MACHINE.SH4: return WinArchitecture.SHX;
            case Kernel32.IMAGE_FILE_MACHINE.SH5: return WinArchitecture.SHX;
            case Kernel32.IMAGE_FILE_MACHINE.ARM: return WinArchitecture.ARM;
            case Kernel32.IMAGE_FILE_MACHINE.ARM_THUMB: return WinArchitecture.ARM;
            case Kernel32.IMAGE_FILE_MACHINE.ARMNT: return WinArchitecture.ARM;
            case Kernel32.IMAGE_FILE_MACHINE.ARM64: return WinArchitecture.ARM64;
            case Kernel32.IMAGE_FILE_MACHINE.AM33: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.POWERPC: return WinArchitecture.PPC;
            case Kernel32.IMAGE_FILE_MACHINE.POWERPCFP: return WinArchitecture.PPC;
            case Kernel32.IMAGE_FILE_MACHINE.IA64: return WinArchitecture.IA64;
            case Kernel32.IMAGE_FILE_MACHINE.MIPS16: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.MIPSFPU: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.MIPSFPU16: return WinArchitecture.Mips;
            case Kernel32.IMAGE_FILE_MACHINE.TRICORE: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.AMD64: return WinArchitecture.x64;
            case Kernel32.IMAGE_FILE_MACHINE.M32R: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.CEF: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.CEE: return WinArchitecture.Unknown;
            case Kernel32.IMAGE_FILE_MACHINE.EBC: return WinArchitecture.Unknown;
            default: return WinArchitecture.Unknown;
            }
        }

        private static WinArchitecture FromProcessorArchitecture(ushort processorArchitecture)
        {
            switch (processorArchitecture) {
            case Kernel32.PROCESSOR_ARCHITECTURE.INTEL: return WinArchitecture.x86;
            case Kernel32.PROCESSOR_ARCHITECTURE.MIPS: return WinArchitecture.Mips;
            case Kernel32.PROCESSOR_ARCHITECTURE.ALPHA: return WinArchitecture.Alpha;
            case Kernel32.PROCESSOR_ARCHITECTURE.PPC: return WinArchitecture.PPC;
            case Kernel32.PROCESSOR_ARCHITECTURE.SHX: return WinArchitecture.SHX;
            case Kernel32.PROCESSOR_ARCHITECTURE.ARM: return WinArchitecture.ARM;
            case Kernel32.PROCESSOR_ARCHITECTURE.IA64: return WinArchitecture.IA64;
            case Kernel32.PROCESSOR_ARCHITECTURE.MSIL: return WinArchitecture.MSIL;
            case Kernel32.PROCESSOR_ARCHITECTURE.AMD64: return WinArchitecture.x64;
            case Kernel32.PROCESSOR_ARCHITECTURE.ARM64: return WinArchitecture.ARM64;
            default: return WinArchitecture.Unknown;
            }
        }

        private void DetectWin10()
        {
            if (MajorVersion != 10) return;

            try {
                IRegistryKey rk = m_WinVersion.OpenSubKey("HKLM", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                if (rk is not null) {
                    using (rk) {
                        object ubrobj = rk.GetValue("UBR");
                        if (ubrobj is int ubr) {
                            UpdateBuildNumber = ubr;
                        }
                    }
                }
            } catch (SecurityException) {              // Ignore that we can't access the key
            } catch (UnauthorizedAccessException) {    // Ignore that we can't access the key
            } catch (System.IO.IOException) {          // Should never occur
            } catch (FormatException) {                // Ignore invalid registry value
            } catch (OverflowException) {              // Ignore invalid registry value
            }
        }

        /// <summary>
        /// Calculates the Windows version for a non-exact match.
        /// </summary>
        /// <param name="lastMatch">The last match.</param>
        /// <returns>A string that should be presented to the user.</returns>
        /// <remarks>
        /// This class has an internal database of Windows Versions obtained from MSDN. It might be that the internal
        /// database is out of date, applications can override this to provide more accurate information.
        /// </remarks>
        protected override string CalculateWinVersion(WinVersion lastMatch)
        {
            try {
                string release = m_WinVersion.BrandingFormatString("%WINDOWS_LONG%");
                if (!string.IsNullOrWhiteSpace(release)) return release;
            } catch (EntryPointNotFoundException) {
                /* Ignore the exception and use the default implementation */
            }

            return base.CalculateWinVersion(lastMatch);
        }
    }
}
