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
                Assert.That(version.Version, Is.Not.Null);
                Assert.That(version.Description, Is.Not.Null);
                Assert.That(version.IsValid, Is.True);
                Assert.That(version.FrameworkVersion, Is.Not.Null);

                Console.WriteLine($"Version .NET: {version.Version} ({version.Description}); FrameworkVersion: {version.FrameworkVersion}");
                if (version is NetFx45 version45) {
                    Console.WriteLine($"  Release: {version45.Net45Release}; TargetVersion: {version45.TargetVersion}");
                }
            }
        }
    }
}
