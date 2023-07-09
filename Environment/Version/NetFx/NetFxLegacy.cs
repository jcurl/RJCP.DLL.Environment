namespace RJCP.Core.Environment.Version.NetFx
{
    using System;
    using System.Security;
    using System.Text;
    using Microsoft.Win32;
    using Resources;

    /// <summary>
    /// Older version of .NET 2.0 to 3.5
    /// </summary>
    public sealed class NetFxLegacy : INetVersion
    {
        internal NetFxLegacy(string key)
        {
            if (!key.StartsWith("v")) {
                IsValid = false;
                return;
            }

            try {
                GetNetFxDetails(key, null);
            } catch (SecurityException) {
                IsValid = false;
            } catch (Exception) {
                IsValid = false;
            }
        }

        internal NetFxLegacy(string key, string profile)
        {
            if (!key.StartsWith("v")) {
                IsValid = false;
                return;
            }

            try {
                GetNetFxDetails(key, profile);
            } catch (SecurityException) {
                IsValid = false;
            } catch (Exception) {
                IsValid = false;
            }
        }

        private void GetNetFxDetails(string key, string profile)
        {
            string fullKeyPath;
            if (profile == null) {
                fullKeyPath = string.Format(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\{0}", key);
            } else {
                fullKeyPath = string.Format(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\{0}\{1}", key, profile);
            }

            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(fullKeyPath)) {
                IsValid = false;
                if (registryKey == null) return;
                if (!NetVersions.IsInstalled(registryKey)) return;

#if NETFRAMEWORK
                string keyVersion = key.Substring(1);
#else
                string keyVersion = key[1..];
#endif
                string netVersion = (string)registryKey.GetValue("Version") ?? keyVersion;
                Version parsedNetVersion = NetVersions.GetVersion(netVersion);
                if (parsedNetVersion == null) return;

                if (parsedNetVersion.Major == 4) {
                    // With .NET 4.5 and later, the version is the newest version. On Windows XP, it should be
                    // 4.0.30319. Note, this class is only run for .NET 4.0 and earlier, but is a prerequisite for .NET
                    // 4.5 and later.
                    const string profilePath = @"SOFTWARE\Microsoft\.NETFramework\Policy\v4.0";
                    int maxVersion = 0;
                    using (RegistryKey profileKey = Registry.LocalMachine.OpenSubKey(profilePath)) {
                        if (profileKey != null) {
                            foreach (string versionKeyName in profileKey.GetValueNames()) {
                                if (int.TryParse(versionKeyName, out int buildVersion)) {
                                    if (maxVersion < buildVersion)
                                        maxVersion = buildVersion;
                                }
                            }
                        }
                    }
                    if (maxVersion == 0) {
                        netVersion = new Version(4, 0).ToString();
                    } else {
                        netVersion = new Version(4, 0, maxVersion).ToString();
                    }
                    FrameworkVersion = new Version(4, 0);
                } else {
                    Version parsedKeyVersion = new Version(keyVersion);
                    FrameworkVersion = new Version(parsedKeyVersion.Major, parsedKeyVersion.Minor);
                }

                string servicePack = registryKey.GetValue("SP", "").ToString();
                if (servicePack.Equals("0")) servicePack = string.Empty;

                StringBuilder version = new StringBuilder();
                version.Append(keyVersion);
                if (profile != null) version.Append(' ').Append(profile);
                if (!string.IsNullOrEmpty(servicePack)) version.Append(" SP").Append(servicePack);
                Version = version.ToString();

                StringBuilder description = new StringBuilder();
                description.Append(Messages.NetFxLegacy).Append(' ').Append(netVersion);
                if (profile != null)
                    description.Append(' ').Append(Messages.NetFxLegacyProfile).Append(' ').Append(profile);
                if (!string.IsNullOrEmpty(servicePack))
                    description.Append(' ').Append(Messages.NetFxLegacySp).Append(servicePack);
                Description = description.ToString();
                Profile = profile;

                IsValid = true;
            }
        }

        /// <summary>
        /// Gets the version of .NET as read from the registry.
        /// </summary>
        /// <value>The version of .NET as read from the registry.</value>
        public Version FrameworkVersion { get; private set; }

        /// <summary>
        /// Returns <see langword="true"/> if the version information contains valid (even if partially) information.
        /// </summary>
        /// <value><see langword="true"/> if this instance is valid; otherwise, <see langword="false"/>.</value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the description of the .NET version installed.
        /// </summary>
        /// <value>The .NET version description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the version string for the .NET version installed.
        /// </summary>
        /// <value>The .NET version string.</value>
        public string Version { get; private set; }

        /// <summary>
        /// Defines the profile for use.
        /// </summary>
        /// <value>The profile for use.</value>
        public string Profile { get; private set; }
    }
}
