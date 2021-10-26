namespace RJCP.NetVersion
{
    using System;
    using RJCP.Core.Environment.Version;
    using RJCP.Core.Environment.Version.NetFx;

    static class Program
    {
        static int Main()
        {
            NetVersions versions = new NetVersions();

            Console.WriteLine("Installed versions of .NET");
            Console.WriteLine("================================");

            foreach (INetVersion version in versions) {
                Console.WriteLine("Version .NET: {0} ({1})", version.Version, version.Description);
                if (version is NetFx45 version45) {
                    Console.WriteLine("  Release: {0}; TargetVersion: {1}; Version: {2}",
                        version45.Net45Release, version45.TargetVersion, version45.NetVersion);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Running on Framework");
            Console.WriteLine("================================");
            foreach (INetVersion version in NetVersions.Runtime) {
                Console.WriteLine("Version .NET: {0} ({1})", version.Version, version.Description);
            }

            return 0;
        }
    }
}
