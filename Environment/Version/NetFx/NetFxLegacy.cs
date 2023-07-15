namespace RJCP.Core.Environment.Version.NetFx
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Text;
    using Microsoft.Win32;
    using Resources;

    /// <summary>
    /// Older version of .NET 2.0 to 4.0.
    /// </summary>
    public sealed class NetFxLegacy : INetVersion
    {
        internal static IList<INetVersion> FindNetFxLegacy()
        {
            List<INetVersion> installed = new List<INetVersion>();

            string netKey = NetVersions.GetNetKey(@"NET Framework Setup\NDP\");
            try {
                using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(netKey)) {
                    if (ndpKey == null) return installed;
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames()) {
                        if (NetVersions.IsValidVersion(versionKeyName)) {
                            using (RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName)) {
                                if (versionKey == null) continue;
                                string defaultValue = versionKey.GetValue(null, "").ToString();
                                if (defaultValue != null &&
                                    defaultValue.Equals("deprecated", StringComparison.InvariantCultureIgnoreCase)) continue;

                                if (NetVersions.IsInstalled(versionKey)) {
                                    NetFxLegacy netfx = new NetFxLegacy(versionKeyName);
                                    if (netfx.IsValid) installed.Add(netfx);
                                    continue;
                                }

                                // For .NET 4.0, this covers the "Client" and "Full" profile.
                                foreach (string subKeyName in versionKey.GetSubKeyNames()) {
                                    using (RegistryKey subKey = versionKey.OpenSubKey(subKeyName)) {
                                        if (subKey == null) continue;
                                        if (NetVersions.IsInstalled(subKey)) {
                                            NetFxLegacy netfx = new NetFxLegacy(versionKeyName, subKeyName);
                                            if (netfx.IsValid) installed.Add(netfx);
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

            return installed;
        }

        internal NetFxLegacy(string key) : this(key, null) { }

        internal NetFxLegacy(string key, string profile)
        {
            FrameworkVersion = NetVersions.GetVersion(key);
            if (FrameworkVersion == null) return;

            try {
                GetNetFxDetails(key, profile);
            } catch (SecurityException) {
                IsValid = false;
            }
        }

        private void GetNetFxDetails(string key, string profile)
        {
            string fullKeyPath;
            if (profile == null) {
                fullKeyPath = NetVersions.GetNetKey($@"NET Framework Setup\NDP\{key}");
            } else {
                fullKeyPath = NetVersions.GetNetKey($@"NET Framework Setup\NDP\{key}\{profile}");
            }

            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(fullKeyPath)) {
                if (registryKey == null) return;

                string installVersion = (string)registryKey.GetValue("Version");
                if (installVersion == null) {
                    InstallVersion = FrameworkVersion;
                } else {
                    InstallVersion = NetVersions.GetVersion(installVersion);
                }

                Profile = profile;
                string servicePack = registryKey.GetValue("SP", "").ToString();
                if (servicePack.Equals("0")) servicePack = string.Empty;

                StringBuilder description = new StringBuilder();
                description.Append(Messages.NetFxLegacy).Append(" v").Append(FrameworkVersion.ToString());
                if (profile != null)
                    description.Append(' ').Append(Messages.NetFxLegacyProfile).Append(' ').Append(profile);
                if (!string.IsNullOrEmpty(servicePack))
                    description.Append(' ').Append(Messages.NetFxLegacySp).Append(servicePack);
                Description = description.ToString();
                IsValid = true;
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
        public DotNetVersionType VersionType { get { return DotNetVersionType.NetFx; } }

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
        /// Defines the profile for use.
        /// </summary>
        /// <value>The profile for use.</value>
        public string Profile { get; private set; }
    }
}
