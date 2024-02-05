namespace RJCP.Core.Environment.Version.NetFx.Runtime
{
    using System;
#if NETFRAMEWORK
    using System.Reflection;
    using Resources;
#endif

    /// <summary>
    /// Description about the current running runtime.
    /// </summary>
    public sealed class NetRuntime : INetVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetRuntime"/> class.
        /// </summary>
        internal NetRuntime()
        {
            FrameworkVersion = Environment.Version;
            InstallVersion = Environment.Version;

#if NET6_0_OR_GREATER
            VersionType = DotNetVersionType.NetCore;
            Description = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#else
            VersionType = DotNetVersionType.NetFx;

            // This information is available in .NET 4.7.1 and later. But as we also target earlier frameworks, get this
            // information dynamically.
            Type rti = Type.GetType("System.Runtime.InteropServices.RuntimeInformation");
            if (rti != null) {
                PropertyInfo property = rti.GetProperty("FrameworkDescription");
                Description = property.GetValue(null, null) as string;
            }

            if (Description == null)
                Description = Messages.NetFxRuntime;
#endif
            IsValid = true;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the version information contains valid (even if partially) information.
        /// </summary>
        /// <value><see langword="true"/> if this instance is valid; otherwise, <see langword="false"/>.</value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// The .NET Version Type.
        /// </summary>
        public DotNetVersionType VersionType { get; private set; }

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
    }
}
