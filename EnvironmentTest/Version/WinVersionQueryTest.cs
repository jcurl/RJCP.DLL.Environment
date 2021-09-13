namespace RJCP.Core.Environment.Version
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class WinVersionQueryTest
    {
        [Test]
        [Platform(Include = "Win32NT")]
        public void OSVersionCheckCurrentOS()
        {
            WinVersion current = WinVersion.LocalMachine;
            Console.WriteLine($"{current}");
            Console.WriteLine($"WinVersionString: {current.WinVersionString}");
            Console.WriteLine($"VersionString: {current.VersionString}");
            Console.WriteLine($"Major.Minor.Build: {current.MajorVersion}.{current.MinorVersion}.{current.BuildNumber}");
            Console.WriteLine($"PlatformId: {current.PlatformId}");
            Console.WriteLine($"PlatformIdString: {current.PlatformIdString}");
            Console.WriteLine($"ProductInfo: {current.ProductInfo}");
            Console.WriteLine($"ProductInfoString: {current.ProductInfoString}");
            Console.WriteLine($"ProductType: {current.ProductType}");
            Console.WriteLine($"ProductTypeString: {current.ProductTypeString}");
            Console.WriteLine($"Suite Flags: {current.SuiteFlags:X}");
            Console.WriteLine($"SuiteString: {current.SuiteString}");
            Console.WriteLine($"Architecture: {current.Architecture}");
            Console.WriteLine($"CSD Version: {current.CSDVersion}");
            Console.WriteLine($"Server R2: {current.ServerR2}");
            Console.WriteLine($"Service Pack: {current.ServicePackMajor}.{current.ServicePackMinor}");
            Assert.Inconclusive("Please check output that it matches your computer");
        }

        [Test]
        [Platform(Include = "Unix")]
        public void OSVersionCheckCurrentOSNotSupported()
        {
            Assert.That(() => {
                _ = WinVersion.LocalMachine;
            }, Throws.TypeOf<PlatformNotSupportedException>());
        }
    }
}
