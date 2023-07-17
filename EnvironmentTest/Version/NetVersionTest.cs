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
            NetVersions versions = new NetVersions();
            foreach (INetVersion version in versions) {
                Assert.That(version.IsValid, Is.True);
                Assert.That(version.FrameworkVersion, Is.Not.Null);
                Assert.That(version.InstallVersion, Is.Not.Null);
                Assert.That(version.Description, Is.Not.Null);

                Console.WriteLine($"Version .NET: {version.FrameworkVersion} ({version.Description}); Install Version: {version.InstallVersion}");
                if (version is NetFx45 version45)
                    Console.WriteLine($"  Release: {version45.Net45Release}; TargetVersion: {version45.TargetVersion}");
                if (version is Mono mono)
                    Console.WriteLine($"  mscorlib: {mono.MsCorLibVersion}; Path: {mono.MonoPath}");
            }
        }

        [Test]
        public void RuntimeVersions()
        {
            foreach (INetVersion version in NetVersions.Runtime) {
                Assert.That(version.IsValid, Is.True);
                Assert.That(version.FrameworkVersion, Is.Not.Null);
                Assert.That(version.InstallVersion, Is.Not.Null);
                Assert.That(version.Description, Is.Not.Null);

                Console.WriteLine($"Version .NET: {version.FrameworkVersion} ({version.Description}); Install Version: {version.InstallVersion}");
                if (version is NetFx45 version45) {
                    Console.WriteLine($"  Release: {version45.Net45Release}; TargetVersion: {version45.TargetVersion}");
                }
            }
        }
    }
}
