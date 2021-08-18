namespace RJCP.Core.Environment
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class XdgTest
    {
        private const string XdgDataHome = "XDG_DATA_HOME";
        private const string XdgConfigHome = "XDG_CONFIG_HOME";
        private const string XdgCacheHome = "XDG_CACHE_HOME";

        [Test]
        public void XdgPrintVariables()
        {
            foreach (Xdg.SpecialFolder e in Enum.GetValues(typeof(Xdg.SpecialFolder))) {
                Console.WriteLine("{0} = {1}", e.ToString(), Xdg.GetFolderPath(e));
            }
        }

        [TestCase(XdgDataHome, Xdg.SpecialFolder.LocalApplicationData, ".local", "share", TestName = "XdgLocalApplicationData")]
        [TestCase(XdgConfigHome, Xdg.SpecialFolder.CommonApplicationData, ".config", TestName = "XdgCommonApplicationData")]
        [TestCase(XdgCacheHome, Xdg.SpecialFolder.CacheData, ".cache", TestName = "XdgCacheData")]
        public void XdgLocalApplicationData(string envVar, Xdg.SpecialFolder folder, params string[] path)
        {
            string xdgData = Environment.GetEnvironmentVariable(envVar);
            try {
                Environment.SetEnvironmentVariable(envVar, null);
                string userHome = GetHomeDirectory();

                string fullPath = Combine(userHome, path);
                string result = Xdg.GetFolderPath(folder);
                Assert.That(result, Is.EqualTo(fullPath));
            } finally {
                Environment.SetEnvironmentVariable(envVar, xdgData);
            }
        }

        [TestCase(XdgDataHome, Xdg.SpecialFolder.LocalApplicationData, ".local", "share", TestName = "XdgLocalApplicationDataEnv")]
        [TestCase(XdgConfigHome, Xdg.SpecialFolder.CommonApplicationData, ".config", TestName = "XdgCommonApplicationDataEnv")]
        [TestCase(XdgCacheHome, Xdg.SpecialFolder.CacheData, ".cache", TestName = "XdgCacheDataEnv")]
        public void XdgLocalApplicationDataEnvSet(string envVar, Xdg.SpecialFolder folder, params string[] path)
        {
            string xdgData = Environment.GetEnvironmentVariable(envVar);
            try {
                string baseDir = Path.Combine(GetHomeDirectory(), "tmp");
                string xdgDir = Combine(baseDir, path);
                Environment.SetEnvironmentVariable(envVar, xdgDir);

                string result = Xdg.GetFolderPath(folder);
                Assert.That(result, Is.EqualTo(xdgDir));
            } finally {
                Environment.SetEnvironmentVariable(envVar, xdgData);
            }
        }

        [TestCase(XdgDataHome, Xdg.SpecialFolder.LocalApplicationData, ".local", "share", TestName = "XdgLocalApplicationDataResolveFileNone")]
        [TestCase(XdgConfigHome, Xdg.SpecialFolder.CommonApplicationData, ".config", TestName = "XdgCommonApplicationDataResolveFileNone")]
        [TestCase(XdgCacheHome, Xdg.SpecialFolder.CacheData, ".cache", TestName = "XdgCacheDataResolveFileNone")]
        public void XdgResolveFile(string envVar, Xdg.SpecialFolder folder, params string[] path)
        {
            string xdgData = Environment.GetEnvironmentVariable(envVar);
            try {
                Environment.SetEnvironmentVariable(envVar, null);
                string userHome = GetHomeDirectory();

                // It is expected that this file doesn't exist in the XDG_*_DIRS path.
                string fullPath = Path.Combine(Combine(userHome, path), "foo.txt");
                string result = Xdg.ResolveFile(folder, "foo.txt");
                Assert.That(result, Is.EqualTo(fullPath));
            } finally {
                Environment.SetEnvironmentVariable(envVar, xdgData);
            }
        }

        [TestCase(XdgDataHome, Xdg.SpecialFolder.LocalApplicationData, ".local", "share", TestName = "XdgLocalApplicationDataResolveDirNone")]
        [TestCase(XdgConfigHome, Xdg.SpecialFolder.CommonApplicationData, ".config", TestName = "XdgCommonApplicationDataResolveDirNone")]
        [TestCase(XdgCacheHome, Xdg.SpecialFolder.CacheData, ".cache", TestName = "XdgCacheDataResolveDirNone")]
        public void XdgResolveDirectory(string envVar, Xdg.SpecialFolder folder, params string[] path)
        {
            string xdgData = Environment.GetEnvironmentVariable(envVar);
            try {
                Environment.SetEnvironmentVariable(envVar, null);
                string userHome = GetHomeDirectory();

                // It is expected that this directory doesn't exist in the XDG_*_DIRS path.
                string fullPath = Path.Combine(Combine(userHome, path), "foo");
                string result = Xdg.ResolveDirectory(folder, "foo");
                Assert.That(result, Is.EqualTo(fullPath));
            } finally {
                Environment.SetEnvironmentVariable(envVar, xdgData);
            }
        }

        private static string GetHomeDirectory()
        {
            if (Platform.IsUnix()) return Environment.GetEnvironmentVariable("HOME");
            if (Platform.IsWinNT()) return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return string.Empty;
        }

        private static string Combine(string path, string[] paths)
        {
            string[] result = new string[paths.Length + 1];
            result[0] = path;
            for (int i = 1; i <= paths.Length; i++) {
                result[i] = paths[i - 1];
            }
            return Path.Combine(result);
        }
    }
}
