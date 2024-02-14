namespace RJCP.Core.Environment.Version
{
    using System;
    using NetFx;
    using NUnit.Framework;

    [TestFixture]
    public class NetVersionTest
    {
        [Test]
        public void ReadAllVersions()
        {
            NetVersions versions = new();
            foreach (INetVersion version in versions) {
                Assert.That(version.IsValid, Is.True);
                Assert.That(version.FrameworkVersion, Is.Not.Null);
                Assert.That(version.InstallVersion, Is.Not.Null);
                Assert.That(version.Description, Is.Not.Null);

                Console.WriteLine($"Version .NET: {version.FrameworkVersion} ({version.Description}); Install Version: {version.InstallVersion}");
                if (version is NetFx45 version45)
                    Console.WriteLine($"  Release: {version45.Net45Release}; TargetVersion: {version45.TargetVersion}");
                if (version is INetVersionMono mono)
                    Console.WriteLine($"  mscorlib: {mono.MsCorLibVersion}; Architecture: {mono.Architecture}; Path: {mono.MonoPath}");
            }
        }

        [Test]
        public void RuntimeVersions()
        {
            Assert.That(NetVersions.Runtime.IsValid, Is.True);
            Assert.That(NetVersions.Runtime.FrameworkVersion, Is.Not.Null);
            Assert.That(NetVersions.Runtime.InstallVersion, Is.Not.Null);
            Assert.That(NetVersions.Runtime.Description, Is.Not.Null);

            Console.WriteLine("");
            Console.WriteLine("Running on Framework");
            Console.WriteLine("================================");
            Console.WriteLine("Version .NET: {0} ({1}); Install Version: {2}",
                NetVersions.Runtime.FrameworkVersion, NetVersions.Runtime.Description, NetVersions.Runtime.InstallVersion);
            if (NetVersions.Runtime is INetVersionMono monoRuntime)
                Console.WriteLine("  mscorlib: {0}; Architecture: {1}; Path: {2}",
                    monoRuntime.MsCorLibVersion, monoRuntime.Architecture, monoRuntime.MonoPath);
        }
    }
}
