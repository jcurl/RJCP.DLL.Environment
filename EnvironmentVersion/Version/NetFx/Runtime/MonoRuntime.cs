namespace RJCP.Core.Environment.Version.NetFx.Runtime
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// An instance checking if the .NET Run Time is MONO on Windows or Unix.
    /// </summary>
    public sealed class MonoRuntime : INetVersionMono
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoRuntime"/> class.
        /// </summary>
        internal MonoRuntime()
        {
            GetMonoVersion();
        }

        private void GetMonoVersion()
        {
            Type monoType = Type.GetType("Mono.Runtime");
            if (monoType == null) {
                IsValid = false;
                return;
            }

            FrameworkVersion = Environment.Version;

            // Don't know how to get the version of the Mono Runtime internally.
            InstallVersion = new Version(0, 0);

            Assembly monoAssembly = Assembly.GetAssembly(monoType);
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(monoAssembly.Location);
            if (Version.TryParse(fvi.FileVersion, out Version fileVersion)) {
                MsCorLibVersion = fileVersion;
            } else {
                MsCorLibVersion = new Version(0, 0);
            }

            // Usually starts of as "C:\ProgramFiles\Mono\lib\4.5\mscorlib.dll".
            string binPath = Path.GetDirectoryName(monoAssembly.Location);
            int i = 4;
            do {
                string testPath = TestPath(binPath, "mono.exe");
                if (testPath != null) {
                    MonoPath = testPath;
                    continue;
                }

                testPath = TestPath(binPath, "mono");
                if (testPath != null) {
                    MonoPath = testPath;
                    continue;
                }

                binPath = Path.GetDirectoryName(binPath);
                i--;
            } while (MonoPath == null && i > 0);

            MethodInfo displayName = monoType.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
            if (displayName != null) {
                Description = string.Format("Mono {0}", displayName.Invoke(null, null));
            }

            if (Platform.IsWinNT()) {
                if (Environment.Is64BitOperatingSystem) {
                    if (Environment.Is64BitProcess) {
                        Architecture = "x64";
                    } else {
                        Architecture = "x86";
                    }
                }
            } else {
                Architecture = "x86";
            }

            IsValid = true;
        }

        private static string TestPath(string path, string monoName)
        {
            string testPath = Path.Combine(path, "bin", monoName);
            if (File.Exists(testPath)) return testPath;
            return null;
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
