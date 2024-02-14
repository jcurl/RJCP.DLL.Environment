namespace RJCP.Core.Environment
{
    using System;
    using System.Runtime.Versioning;
    using NUnit.Framework;

    [TestFixture]
    public class PlatformTest
    {
        [Test]
        public void IsWinNT()
        {
            bool result = Platform.IsWinNT();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                Assert.That(result, Is.True);
            } else {
                Assert.That(result, Is.False);
            }
        }

        [Test]
        public void IsUnix()
        {
            int platform = (int)Environment.OSVersion.Platform;
            bool result = Platform.IsUnix();

            // Check http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
            if (platform is 4 or 6 or 128) {
                Assert.That(result, Is.True);
            } else {
                Assert.That(result, Is.False);
            }
        }

        [Test]
        [Platform(Include = "Win32NT")]
        [SupportedOSPlatform("windows")]
        public void SpecialFolderLocalApplicationDirectoryWin32()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Assert.That(Platform.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Is.EqualTo(dir));
        }

        [Test]
        [Platform(Include = "Unix")]
        [SupportedOSPlatform("windows")]
        public void SpecialFolderLocalApplicationDirectoryUnix()
        {
            string dir = Xdg.GetFolderPath(Xdg.SpecialFolder.LocalApplicationData);
            Assert.That(Platform.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Is.EqualTo(dir));
        }

        [Test]
        [Platform(Include = "Win32NT")]
        [SupportedOSPlatform("windows")]
        public void SpecialFolderCommonApplicationDirectoryWin32()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            Assert.That(Platform.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Is.EqualTo(dir));
        }

        [Test]
        [Platform(Include = "Unix")]
        [SupportedOSPlatform("windows")]
        public void SpecialFolderCommonApplicationDirectoryUnix()
        {
            string dir = Xdg.GetFolderPath(Xdg.SpecialFolder.CommonApplicationData);
            Assert.That(Platform.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Is.EqualTo(dir));
        }

        [Test]
        public void SpecialFolderUserProfileDirectory()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Assert.That(Platform.GetFolderPath(Environment.SpecialFolder.UserProfile), Is.EqualTo(dir));
        }
    }
}
