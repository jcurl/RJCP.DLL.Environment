namespace RJCP.Core.Environment.Version.NetFx
{
    using System;
    using Microsoft.Win32;
    using Resources;

    /// <summary>
    /// Installation details for .NET 1.0.
    /// </summary>
    public sealed class NetFx10 : INetVersion
    {
        internal NetFx10(string version, string key)
        {
            if (version.StartsWith("v")) {
#if NETFRAMEWORK
                Version = version.Substring(1);
#else
                Version = version[1..];
#endif
            } else {
                Version = version;
            }
            GetNetFxDetails(key);
        }

        private void GetNetFxDetails(string key)
        {
            string fullKeyPath = string.Format(@"SOFTWARE\Microsoft\NET Framework Setup\Product\{0}", key);
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(fullKeyPath)) {
                Package = registryKey.GetValue("Package", "").ToString();
                string netRegistryVersion = registryKey.GetValue("Version", "").ToString();
                Description = string.Format(Messages.NetFxDetails, netRegistryVersion, Package);
                FrameworkVersion = new Version(1, 0);
                IsValid = FrameworkVersion != null;
            }
        }

        /// <summary>
        /// Gets the package for .NET 1.0 as read from the registry.
        /// </summary>
        /// <value>The package as read from the registry.</value>
        public string Package { get; private set; }

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
        /// Gets the version that can be used for comparison.
        /// </summary>
        /// <value>The .NET version that can be used for comparison.</value>
        public Version FrameworkVersion { get; private set; }
    }
}
