﻿namespace RJCP.Core.Environment.Version.NetFx.Net45
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class NetFxConfig
    {
        // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        private readonly static Dictionary<int, NetFxVersion> Versions = new Dictionary<int, NetFxVersion>() {
            { 378389, new NetFxVersion("4.5", ".NET Framework 4.5") },
            { 378675, new NetFxVersion("4.5.1", ".NET Framework 4.5.1 installed with Windows 8.1 or Windows Server 2012 R2") },
            { 378758, new NetFxVersion("4.5.1", ".NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2") },
            { 379893, new NetFxVersion("4.5.2", ".NET Framework 4.5.2") },
            { 393295, new NetFxVersion("4.6", ".NET Framework 4.6 on Windows 10 (1507)") },
            { 393297, new NetFxVersion("4.6", ".NET Framework 4.6") },
            { 394254, new NetFxVersion("4.6.1", ".NET Framework 4.6.1 nn Windows 10, November Update systems (1511)") },
            { 394271, new NetFxVersion("4.6.1", ".NET Framework 4.6.1") },
            { 394802, new NetFxVersion("4.6.2", ".NET Framework 4.6.2 on Windows 10, Anniversary Update (1607) and Windows Server 2016") },
            { 394806, new NetFxVersion("4.6.2", ".NET Framework 4.6.2") },
            { 460798, new NetFxVersion("4.7", ".NET Framework 4.7 on Windows 10, Creators Update (1703)") },
            { 460805, new NetFxVersion("4.7", ".NET Framework 4.7") },
            { 461308, new NetFxVersion("4.7.1", ".NET Framework 4.7.1 on Windows 10, Fall Creators Update (1709)") },
            { 461310, new NetFxVersion("4.7.1", ".NET Framework 4.7.1") },
            { 461808, new NetFxVersion("4.7.2", ".NET Framework 4.7.2 on Windows 10, Spring Creators Update (1803)") },
            { 461814, new NetFxVersion("4.7.2", ".NET Framework 4.7.2") },
            { 528040, new NetFxVersion("4.8", ".NET Framework 4.8 on Windows 10, May 2019 Update (1903)") },
            { 528049, new NetFxVersion("4.8", ".NET Framework 4.8") },
            { 528209, new NetFxVersion("4.8", ".NET Framework 4.8 on Windows 10, October 2019 Update (1909)") },
            { 528372, new NetFxVersion("4.8", ".NET Framework 4.8 on Windows 10, October 2020 Update (20H2, 21H1") },
            { 528449, new NetFxVersion("4.8", ".NET Framework 4.8 on Windows 11 / Server 2022") }
        };

        public static NetFxVersion GetNetFxVersion(int release)
        {
            if (Versions.TryGetValue(release, out NetFxVersion version)) return version;

            var versionList = from netVersion in Versions
                              orderby netVersion.Key select netVersion;
            NetFxVersion derivedVersion = null;
            foreach (var netVersion in versionList) {
                if (netVersion.Key >= release) {
                    if (derivedVersion == null) {
                        return new NetFxVersion("< 4.5", $"Unknown .NET version before .NET 4.5 ({release})");
                    }
                    derivedVersion = netVersion.Value;
                    break;
                }
                derivedVersion = netVersion.Value;
            }

            // This can't happen, because the enumeration will always have at least one element, but quiesces a possible
            // null pointer warning.
            if (derivedVersion == null) return null;

            return new NetFxVersion(derivedVersion.Version,
                $".NET {derivedVersion.Version} or later, release {release}");
        }
    }
}
