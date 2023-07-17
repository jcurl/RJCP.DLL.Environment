namespace RJCP.Core.Environment.Version.NetFx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Security;
    using Microsoft.Win32;
    using Resources;

    /// <summary>
    /// Installation details for the Mono Runtime.
    /// </summary>
    public sealed class Mono : INetVersion
    {
        private const string MonoKey = @"SOFTWARE\Mono";
        private const string MonoKey86 = @"SOFTWARE\Wow6432Node\Mono";

        internal static IList<INetVersion> FindMonoWindows()
        {
            List<INetVersion> installed = new List<INetVersion>();

            bool installReg = FindMonoWindows(MonoKey, installed);
            if (Environment.Is64BitProcess) installReg |= FindMonoWindows(MonoKey86, installed);

            if (!installReg) {
                string binPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono", "bin", "mono.exe");
                string libPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono", "lib");
                string arch = Environment.Is64BitProcess ? "x64" : "x86";
                FindMonoWindows(binPath, libPath, null, arch, installed);

                if (Environment.Is64BitProcess) {
                    string binPath32 =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono", "bin", "mono.exe");
                    string libPath32 =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono", "lib");
                    FindMonoWindows(binPath32, libPath32, null, "x86", installed);
                }
            }

            return installed;
        }

        internal static bool FindMonoWindows(string key, IList<INetVersion> installed)
        {
            try {
                using (RegistryKey monoKey = Registry.LocalMachine.OpenSubKey(key)) {
                    if (monoKey == null) return false;
                    if (!NetVersions.IsInstalled(monoKey, "Installed")) return false;
                    string sdkDir = monoKey.GetValue("SdkInstallRoot") as string;
                    string monoPath = Path.Combine(sdkDir, "bin", "mono.exe");
                    string assemblyDir = monoKey.GetValue("FrameworkAssemblyDirectory") as string;
                    string version = monoKey.GetValue("Version") as string;
                    string arch = monoKey.GetValue("Architecture") as string;
                    FindMonoWindows(monoPath, assemblyDir, version, arch, installed);
                    return true;
                }
            } catch (SecurityException) {
                return false;
            }
        }

        internal static void FindMonoWindows(string path, string assemblyDir, string version, string arch, IList<INetVersion> installed)
        {
            if (!File.Exists(path)) return;
            if (!Directory.Exists(assemblyDir)) return;

            Version net35 = null;
            Version net11 = GetFileVersion(assemblyDir, @"mono\1.0\mscorlib.dll");
            Version net20 = GetFileVersion(assemblyDir, @"mono\2.0\mscorlib.dll");
            Version net40 = GetFileVersion(assemblyDir, @"mono\4.0\mscorlib.dll");
            Version net45 = GetFileVersion(assemblyDir, @"mono\4.5\mscorlib.dll");
            if (net20 != null) {
                if (Directory.Exists(Path.Combine(assemblyDir, @"mono\3.5"))) {
                    net35 = net20;
                }
            }

            if (!Version.TryParse(version, out Version installVersion))
                installVersion = new Version(0, 0);

            if (net11 != null) installed.Add(new Mono(new Version(1, 1, 4322), net11, installVersion, path, arch));
            if (net20 != null) installed.Add(new Mono(new Version(2, 0), net20, installVersion, path, arch));
            if (net35 != null) installed.Add(new Mono(new Version(3, 5), net35, installVersion, path, arch));
            if (net40 != null) installed.Add(new Mono(new Version(4, 0), net40, installVersion, path, arch));
            if (net45 != null) installed.Add(new Mono(new Version(4, 5), net45, installVersion, path, arch));
        }

        private static Version GetFileVersion(string assemblyDir, string file)
        {
            string fullPath = Path.Combine(assemblyDir, file);
            if (!File.Exists(fullPath)) return null;

            try {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(fullPath);
                if (Version.TryParse(info.FileVersion, out Version version))
                    return version;
                return new Version(0, 0);
            } catch (FileNotFoundException) {
                return null;
            }
        }

        internal Mono(Version framework, Version file, Version install, string path, string arch)
        {
            if (framework == null) return;
            FrameworkVersion = framework;
            InstallVersion = install ?? file;
            MonoPath = path;
            MsCorLibVersion = file;
            Architecture = arch ?? "x86";
            IsValid = true;

            if (InstallVersion != null) {
                Description = string.Format(Messages.Mono, FrameworkVersion.ToString(), InstallVersion.ToString(), arch);
            } else {
                Description = string.Format(Messages.MonoNoVersion, FrameworkVersion.ToString(), arch);
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if the version information contains valid (even if partially) information.
        /// </summary>
        /// <value><see langword="true"/> if this instance is valid; otherwise, <see langword="false"/>.</value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// The .NET Version Type.
        /// </summary>
        public DotNetVersionType VersionType { get { return DotNetVersionType.Mono; } }

        /// <summary>
        /// Gets the version that can be used for comparison.
        /// </summary>
        /// <value>The .NET version that can be used for comparison.</value>
        public Version FrameworkVersion { get; private set; }

        /// <summary>
        /// Gets the version of the installation.
        /// </summary>
        /// <value>The .NET installation version.</value>
        public Version InstallVersion { get; private set; }

        /// <summary>
        /// Gets the description of the .NET version installed.
        /// </summary>
        /// <value>The .NET version description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the path to the Mono binary.
        /// </summary>
        public string MonoPath { get; private set; }

        /// <summary>
        /// Gets the version of the MSCorLib.DLL file installed.
        /// </summary>
        public Version MsCorLibVersion { get; private set; }

        /// <summary>
        /// Gets the architecture for the Mono runtime.
        /// </summary>
        public string Architecture { get; private set; }
    }
}
