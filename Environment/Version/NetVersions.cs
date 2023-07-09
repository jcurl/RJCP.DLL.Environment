namespace RJCP.Core.Environment.Version
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security;
    using Microsoft.Win32;
    using NetFx.Runtime;

    /// <summary>
    /// A class to get the versions of .NET installed on the local computer. Only relevant for Windows.
    /// </summary>
    public sealed class NetVersions : IEnumerable<INetVersion>
    {
        private readonly List<INetVersion> m_Installed = new List<INetVersion>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NetVersions"/> class. Other operating systems will just list no
        /// versions of .NET (until there might be support at a later date).
        /// </summary>
        public NetVersions()
        {
            if (Platform.IsWinNT()) {
                FindNetFxVersions();
            }
        }

        private void FindNetFxVersions()
        {
            FindNetFx10();
            FindNetFxLegacy();

            try {
                NetFx.NetFx45 version45 = new NetFx.NetFx45();
                if (version45.IsValid) m_Installed.Add(version45);
            } catch (SecurityException) {
                /* Ignore */
            }
        }

        private void FindNetFx10()
        {
            try {
                using (RegistryKey netKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\Full\")) {
                    if (netKey == null) return;
                    foreach (string versionKeyName in netKey.GetSubKeyNames()) {
                        if (versionKeyName.StartsWith("v")) {
                            using (RegistryKey versionKey = netKey.OpenSubKey(versionKeyName)) {
                                foreach (string langCode in versionKey.GetSubKeyNames()) {
                                    using (RegistryKey langKey = versionKey.OpenSubKey(langCode)) {
                                        foreach (string product in langKey.GetSubKeyNames()) {
                                            using (RegistryKey productKey = langKey.OpenSubKey(product)) {
                                                if (IsInstalled(productKey)) {
                                                    NetFx.NetFx10 net10 = new NetFx.NetFx10(versionKeyName, product);
                                                    if (net10.IsValid)
                                                        m_Installed.Add(net10);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (SecurityException) {
                /* Ignore */
            }
        }

        // See https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        private void FindNetFxLegacy()
        {
            string netKey;
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess) {
                netKey = @"SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\";
            } else {
                netKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";
            }

            try {
                using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(netKey)) {
                    if (ndpKey == null) return;
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames()) {
                        if (versionKeyName.StartsWith("v")) {
                            using (RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName)) {
                                string defaultValue = versionKey.GetValue(null, "").ToString();
                                if (defaultValue != null && defaultValue.Equals("deprecated")) return;

                                if (IsInstalled(versionKey)) {
                                    NetFx.NetFxLegacy netfx = new NetFx.NetFxLegacy(versionKeyName);
                                    if (netfx.IsValid)
                                        m_Installed.Add(netfx);
                                    continue;
                                }
                                foreach (string subKeyName in versionKey.GetSubKeyNames()) {
                                    using (RegistryKey subKey = versionKey.OpenSubKey(subKeyName)) {
                                        if (IsInstalled(subKey)) {
                                            NetFx.NetFxLegacy netfx = new NetFx.NetFxLegacy(versionKeyName, subKeyName);
                                            if (netfx.IsValid)
                                                m_Installed.Add(netfx);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (SecurityException) {
                /* Ignore */
            }
        }

        /// <summary>
        /// Determines whether the specified NET Framework is installed.
        /// </summary>
        /// <param name="key">The key for the framework.</param>
        /// <returns>
        /// <see langword="true"/> if the specified key is installed; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool IsInstalled(RegistryKey key)
        {
            string install = key.GetValue("Install", "").ToString();
            return install.Equals("1", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the version of the framework from the string.
        /// </summary>
        /// <param name="version">The version string.</param>
        /// <returns>The version of the framework, or <see langword="null"/> if it is invalid.</returns>
        internal static Version GetVersion(string version)
        {
            try {
                return new Version(version);
            } catch (ArgumentException) {
                return null;
            } catch (FormatException) {
                return null;
            } catch (OverflowException) {
                return null;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection of .NET versions found.</returns>
        public IEnumerator<INetVersion> GetEnumerator()
        {
            return m_Installed.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static readonly object RunTimeLock = new object();
        private static List<INetVersion> RunTimes;

        /// <summary>
        /// Gets the detected .NET runtimes.
        /// </summary>
        /// <value>The current .NET runtimes.</value>
        public static IEnumerable<INetVersion> Runtime
        {
            get
            {
                if (RunTimes == null) {
                    lock (RunTimeLock) {
                        if (RunTimes == null) {
                            RunTimes = new List<INetVersion> {
                                new NetRuntime()
                            };

                            INetVersion mono = new Mono();
                            if (mono.IsValid) RunTimes.Add(mono);
                        }
                    }
                }
                return RunTimes;
            }
        }
    }
}
