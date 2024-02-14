namespace RJCP.NetVersion
{
    using System;
    using RJCP.Core.Environment.Version;
    using RJCP.Core.Environment.Version.NetFx;

    static class Program
    {
        static int Main()
        {
            NetVersions versions = new();

            Console.WriteLine("Installed versions of .NET");
            Console.WriteLine("================================");

            foreach (INetVersion version in versions) {
                Console.WriteLine("Version .NET: {0} ({1}); Install Version {2}",
                    version.FrameworkVersion, version.Description, version.InstallVersion);
                if (version is NetFx45 version45)
                    Console.WriteLine("  Release: {0}; TargetVersion: {1}", version45.Net45Release, version45.TargetVersion);
                if (version is INetVersionMono mono)
                    Console.WriteLine("  mscorlib: {0}; Architecture: {1}; Path: {2}",
                        mono.MsCorLibVersion, mono.Architecture, mono.MonoPath);
            }

            Console.WriteLine("");
            Console.WriteLine("Running on Framework");
            Console.WriteLine("================================");
            Console.WriteLine("Version .NET: {0} ({1}); Install Version: {2}",
                NetVersions.Runtime.FrameworkVersion, NetVersions.Runtime.Description, NetVersions.Runtime.InstallVersion);
            if (NetVersions.Runtime is INetVersionMono monoRuntime)
                Console.WriteLine("  mscorlib: {0}; Architecture: {1}; Path: {2}",
                    monoRuntime.MsCorLibVersion, monoRuntime.Architecture, monoRuntime.MonoPath);

            return 0;
        }
    }
}
