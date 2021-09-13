namespace RJCP.Core.Environment.Version
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class WinVersionTest
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
            Console.WriteLine($"NativeArchitecture: {current.NativeArchitecture}");
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

        [Test]
        public void WinVersionOrder()
        {
            // Desktop build comparisons
            Assert.That(WinVersion.Win32s, Is.LessThan(WinVersion.Win95));
            Assert.That(WinVersion.Win95, Is.LessThan(WinVersion.Win95OSR2));
            Assert.That(WinVersion.Win95OSR2, Is.LessThan(WinVersion.Win98));
            Assert.That(WinVersion.Win98, Is.LessThan(WinVersion.Win98SE));
            Assert.That(WinVersion.Win98SE, Is.LessThan(WinVersion.WinME));
            Assert.That(WinVersion.WinME, Is.LessThan(WinVersion.WinNT351));
            Assert.That(WinVersion.WinNT351, Is.LessThan(WinVersion.WinNT4));
            Assert.That(WinVersion.WinNT4, Is.LessThan(WinVersion.Win2000));
            Assert.That(WinVersion.Win2000, Is.LessThan(WinVersion.WinXP));
            Assert.That(WinVersion.WinXP, Is.LessThan(WinVersion.WinXPx64));
            Assert.That(WinVersion.WinXPx64, Is.LessThan(WinVersion.Vista));
            Assert.That(WinVersion.Vista, Is.LessThan(WinVersion.Win7));
            Assert.That(WinVersion.Win7, Is.LessThan(WinVersion.Win8));
            Assert.That(WinVersion.Win8, Is.LessThan(WinVersion.Win8_1));
            Assert.That(WinVersion.Win8_1, Is.LessThan(WinVersion.Windows10));

            Assert.That(WinVersion.WinXP, Is.EqualTo(WinVersion.WinXPSP0));
            Assert.That(WinVersion.WinXP, Is.EqualTo(WinVersion.WinXPSP1));
            Assert.That(WinVersion.WinXP, Is.EqualTo(WinVersion.WinXPSP2));
            Assert.That(WinVersion.WinXP, Is.EqualTo(WinVersion.WinXPSP3));
            Assert.That(WinVersion.WinXPSP0, Is.LessThan(WinVersion.WinXPSP1));
            Assert.That(WinVersion.WinXPSP1, Is.LessThan(WinVersion.WinXPSP2));
            Assert.That(WinVersion.WinXPSP2, Is.LessThan(WinVersion.WinXPSP3));

            Assert.That(WinVersion.Windows10_1507, Is.LessThan(WinVersion.Windows10_1511));
            Assert.That(WinVersion.Windows10_1511, Is.LessThan(WinVersion.Windows10_1607));
            Assert.That(WinVersion.Windows10_1607, Is.LessThan(WinVersion.Windows10_1703));
            Assert.That(WinVersion.Windows10_1703, Is.LessThan(WinVersion.Windows10_1709));
            Assert.That(WinVersion.Windows10_1709, Is.LessThan(WinVersion.Windows10_1803));
            Assert.That(WinVersion.Windows10_1803, Is.LessThan(WinVersion.Windows10_1809));
            Assert.That(WinVersion.Windows10_1809, Is.LessThan(WinVersion.Windows10_1903));
            Assert.That(WinVersion.Windows10_1903, Is.LessThan(WinVersion.Windows10_1909));
            Assert.That(WinVersion.Windows10_1909, Is.LessThan(WinVersion.Windows10_2004));
            Assert.That(WinVersion.Windows10_2004, Is.LessThan(WinVersion.Windows10_20H2));
            Assert.That(WinVersion.Windows10_20H2, Is.LessThan(WinVersion.Windows10_21H1));

            // Server build comparisons
            Assert.That(WinVersion.Win2000, Is.LessThan(WinVersion.Win2003));
            Assert.That(WinVersion.Win2003, Is.LessThan(WinVersion.Win2003R2));
            Assert.That(WinVersion.Win2003R2, Is.LessThan(WinVersion.Win2008));
            Assert.That(WinVersion.Win2008, Is.LessThan(WinVersion.Win2008R2));
            Assert.That(WinVersion.Win2008R2, Is.LessThan(WinVersion.Win2012));
            Assert.That(WinVersion.Win2012, Is.LessThan(WinVersion.Win2012R2));
            Assert.That(WinVersion.Win2012R2, Is.LessThan(WinVersion.Win2016));
            Assert.That(WinVersion.Win2016, Is.LessThan(WinVersion.Win2019));

            // Desktop and Server build comparisons. Only the version number
            // is compared. if you need to have a server version, then explicitly
            // check that it is the server. Comparing a server version with a
            // desktop version that has the same version number will match
            // * So if you need Windows 7 or better:
            //     if (OSVerQuery.LocalMachine >= WinVersion.Win7) { Windows 7 or server or better }
            // * If you only run on a server (check that you're not a workstation, because you
            //   could have a server or a domain controller)
            //     if (OSVerQuery.Localmachine >= WinVersion.Win2008R2 &&
            //         OSVerQuery.ProductType != WinVersion.WinProductType.Workstation) { Windows Server }
            // * Note for WinNT4, both the workstation and server are the same version
            Assert.That(WinVersion.Win2008, Is.EqualTo(WinVersion.Vista));
            Assert.That(WinVersion.Win2008R2, Is.EqualTo(WinVersion.Win7));
            Assert.That(WinVersion.Win2012, Is.EqualTo(WinVersion.Win8));
            Assert.That(WinVersion.Win2012R2, Is.EqualTo(WinVersion.Win8_1));
            Assert.That(WinVersion.Win2016, Is.EqualTo(WinVersion.Windows10_1607));
            Assert.That(WinVersion.Win2019, Is.EqualTo(WinVersion.Windows10_1809));
        }

        [Test]
        public void WinVersionServer()
        {
            Assert.That(WinVersion.Win32s.IsServer, Is.False);
            Assert.That(WinVersion.WinCE.IsServer, Is.False);
            Assert.That(WinVersion.Win95.IsServer, Is.False);
            Assert.That(WinVersion.Win95OSR2.IsServer, Is.False);
            Assert.That(WinVersion.Win98.IsServer, Is.False);
            Assert.That(WinVersion.Win98SE.IsServer, Is.False);
            Assert.That(WinVersion.WinME.IsServer, Is.False);
            Assert.That(WinVersion.WinNT351.IsServer, Is.False);
            Assert.That(WinVersion.WinNT4.IsServer, Is.False);
            Assert.That(WinVersion.Win2000.IsServer, Is.False);
            Assert.That(WinVersion.WinXP.IsServer, Is.False);
            Assert.That(WinVersion.WinXPx64.IsServer, Is.False);
            Assert.That(WinVersion.Vista.IsServer, Is.False);
            Assert.That(WinVersion.Win7.IsServer, Is.False);
            Assert.That(WinVersion.Win8.IsServer, Is.False);
            Assert.That(WinVersion.Win8_1.IsServer, Is.False);
            Assert.That(WinVersion.Windows10.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1507.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1511.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1607.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1703.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1709.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1803.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1809.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1903.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_1909.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_2004.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_20H2.IsServer, Is.False);
            Assert.That(WinVersion.Windows10_21H1.IsServer, Is.False);

            Assert.That(WinVersion.Win2003.IsServer, Is.True);
            Assert.That(WinVersion.Win2008.IsServer, Is.True);
            Assert.That(WinVersion.Win2008R2.IsServer, Is.True);
            Assert.That(WinVersion.Win2012.IsServer, Is.True);
            Assert.That(WinVersion.Win2012R2.IsServer, Is.True);
            Assert.That(WinVersion.Win2016.IsServer, Is.True);
            Assert.That(WinVersion.Win2019.IsServer, Is.True);
        }

        [Test]
        public void WinVersionFuturistic()
        {
            WinVersion p65 = new WinVersion(WinPlatform.WinNT, 6, 5);
            Assert.That(WinVersion.Win7, Is.LessThan(p65));
            Assert.That(WinVersion.Win2008R2, Is.LessThan(p65));
            Assert.That(WinVersion.Vista, Is.LessThan(p65));
            Assert.That(WinVersion.Win2008, Is.LessThan(p65));
            Assert.That(WinVersion.Win2003, Is.LessThan(p65));
            Assert.That(WinVersion.Win2003R2, Is.LessThan(p65));
            Assert.That(WinVersion.WinXPx64, Is.LessThan(p65));
            Assert.That(WinVersion.WinXP, Is.LessThan(p65));
            Assert.That(WinVersion.Win2000, Is.LessThan(p65));
            Assert.That(WinVersion.WinNT4, Is.LessThan(p65));
            Assert.That(WinVersion.WinNT351, Is.LessThan(p65));
            Assert.That(WinVersion.WinME, Is.LessThan(p65));
            Assert.That(WinVersion.Win98SE, Is.LessThan(p65));
            Assert.That(WinVersion.Win98, Is.LessThan(p65));
            Assert.That(WinVersion.Win95OSR2, Is.LessThan(p65));
            Assert.That(WinVersion.Win95, Is.LessThan(p65));

            WinVersion p70 = new WinVersion(WinPlatform.WinNT, 7, 0);
            Assert.That(WinVersion.Win8, Is.LessThan(p70));
            Assert.That(WinVersion.Win2012, Is.LessThan(p70));
            Assert.That(WinVersion.Win7, Is.LessThan(p70));
            Assert.That(WinVersion.Win2008R2, Is.LessThan(p70));
            Assert.That(WinVersion.Vista, Is.LessThan(p70));
            Assert.That(WinVersion.Win2008, Is.LessThan(p70));
            Assert.That(WinVersion.Win2003, Is.LessThan(p70));
            Assert.That(WinVersion.Win2003R2, Is.LessThan(p70));
            Assert.That(WinVersion.WinXPx64, Is.LessThan(p70));
            Assert.That(WinVersion.WinXP, Is.LessThan(p70));
            Assert.That(WinVersion.Win2000, Is.LessThan(p70));
            Assert.That(WinVersion.WinNT4, Is.LessThan(p70));
            Assert.That(WinVersion.WinNT351, Is.LessThan(p70));
            Assert.That(WinVersion.WinME, Is.LessThan(p70));
            Assert.That(WinVersion.Win98SE, Is.LessThan(p70));
            Assert.That(WinVersion.Win98, Is.LessThan(p70));
            Assert.That(WinVersion.Win95OSR2, Is.LessThan(p70));
            Assert.That(WinVersion.Win95, Is.LessThan(p70));

            WinVersion p100 = new WinVersion(WinPlatform.WinNT, 10, 0);
            Assert.That(WinVersion.Win8, Is.LessThan(p100));
            Assert.That(WinVersion.Win2012, Is.LessThan(p100));
            Assert.That(WinVersion.Win7, Is.LessThan(p100));
            Assert.That(WinVersion.Win2008R2, Is.LessThan(p100));
            Assert.That(WinVersion.Vista, Is.LessThan(p100));
            Assert.That(WinVersion.Win2008, Is.LessThan(p100));
            Assert.That(WinVersion.Win2003, Is.LessThan(p100));
            Assert.That(WinVersion.Win2003R2, Is.LessThan(p100));
            Assert.That(WinVersion.WinXPx64, Is.LessThan(p100));
            Assert.That(WinVersion.WinXP, Is.LessThan(p100));
            Assert.That(WinVersion.Win2000, Is.LessThan(p100));
            Assert.That(WinVersion.WinNT4, Is.LessThan(p100));
            Assert.That(WinVersion.WinNT351, Is.LessThan(p100));
            Assert.That(WinVersion.WinME, Is.LessThan(p100));
            Assert.That(WinVersion.Win98SE, Is.LessThan(p100));
            Assert.That(WinVersion.Win98, Is.LessThan(p100));
            Assert.That(WinVersion.Win95OSR2, Is.LessThan(p100));
            Assert.That(WinVersion.Win95, Is.LessThan(p100));
            Assert.That(WinVersion.Windows10, Is.EqualTo(p100));
            Assert.That(WinVersion.Win2016, Is.EqualTo(p100));

            WinVersion p101 = new WinVersion(WinPlatform.WinNT, 10, 1);
            Assert.That(WinVersion.Win8, Is.LessThan(p101));
            Assert.That(WinVersion.Win2012, Is.LessThan(p101));
            Assert.That(WinVersion.Win7, Is.LessThan(p101));
            Assert.That(WinVersion.Win2008R2, Is.LessThan(p101));
            Assert.That(WinVersion.Vista, Is.LessThan(p101));
            Assert.That(WinVersion.Win2008, Is.LessThan(p101));
            Assert.That(WinVersion.Win2003, Is.LessThan(p101));
            Assert.That(WinVersion.Win2003R2, Is.LessThan(p101));
            Assert.That(WinVersion.WinXPx64, Is.LessThan(p101));
            Assert.That(WinVersion.WinXP, Is.LessThan(p101));
            Assert.That(WinVersion.Win2000, Is.LessThan(p101));
            Assert.That(WinVersion.WinNT4, Is.LessThan(p101));
            Assert.That(WinVersion.WinNT351, Is.LessThan(p101));
            Assert.That(WinVersion.WinME, Is.LessThan(p101));
            Assert.That(WinVersion.Win98SE, Is.LessThan(p101));
            Assert.That(WinVersion.Win98, Is.LessThan(p101));
            Assert.That(WinVersion.Win95OSR2, Is.LessThan(p101));
            Assert.That(WinVersion.Win95, Is.LessThan(p101));
            Assert.That(WinVersion.Win2016, Is.LessThan(p101));
        }

        [Test]
        public void WinVersionUnknown()
        {
            WinVersion ver61 = new WinVersion(WinPlatform.WinNT, 6, 1) {
                PlatformId = WinPlatform.Unknown
            };

            // If the platform is unknown, that field is ignored.
            Assert.That(WinVersion.Win7, Is.Not.LessThan(ver61));
            Assert.That(WinVersion.Win7, Is.Not.GreaterThan(ver61));
            Assert.That(WinVersion.Win7, Is.EqualTo(ver61));
        }

        [Test]
        public void WinVersionString()
        {
            Assert.That(WinVersion.Win95.WinVersionString, Is.EqualTo("Windows 95"));
            Assert.That(WinVersion.Win95OSR2.WinVersionString, Is.EqualTo("Windows 95OSR2"));
            Assert.That(WinVersion.Win98.WinVersionString, Is.EqualTo("Windows 98"));
            Assert.That(WinVersion.Win98SE.WinVersionString, Is.EqualTo("Windows 98SE"));
            Assert.That(WinVersion.WinME.WinVersionString, Is.EqualTo("Windows ME"));
            Assert.That(WinVersion.WinNT351.WinVersionString, Is.EqualTo("Windows NT 3.51"));
            Assert.That(WinVersion.WinNT4.WinVersionString, Is.EqualTo("Windows NT 4"));
            Assert.That(WinVersion.Win2000.WinVersionString, Is.EqualTo("Windows 2000"));
            Assert.That(WinVersion.WinXP.WinVersionString, Is.EqualTo("Windows XP"));
            Assert.That(WinVersion.Win2003.WinVersionString, Is.EqualTo("Windows 2003"));
            Assert.That(WinVersion.Win2003R2.WinVersionString, Is.EqualTo("Windows 2003R2"));
            Assert.That(WinVersion.Vista.WinVersionString, Is.EqualTo("Windows Vista"));
            Assert.That(WinVersion.Win2008.WinVersionString, Is.EqualTo("Windows 2008"));
            Assert.That(WinVersion.Win7.WinVersionString, Is.EqualTo("Windows 7"));
            Assert.That(WinVersion.Win2008R2.WinVersionString, Is.EqualTo("Windows 2008R2"));
            Assert.That(WinVersion.Win8.WinVersionString, Is.EqualTo("Windows 8"));
            Assert.That(WinVersion.Win2012.WinVersionString, Is.EqualTo("Windows 2012"));
            Assert.That(WinVersion.Win8_1.WinVersionString, Is.EqualTo("Windows 8.1"));
            Assert.That(WinVersion.Win2012R2.WinVersionString, Is.EqualTo("Windows 2012R2"));
            Assert.That(WinVersion.Windows10.WinVersionString, Is.EqualTo("Windows 10"));
            Assert.That(WinVersion.Windows10_1507.WinVersionString, Is.EqualTo("Windows 10 v1507 - Threshold 1"));
            Assert.That(WinVersion.Windows10_1511.WinVersionString, Is.EqualTo("Windows 10 v1511 - Threshold 2"));
            Assert.That(WinVersion.Windows10_1607.WinVersionString, Is.EqualTo("Windows 10 v1607 - Redstone 1"));
            Assert.That(WinVersion.Windows10_1703.WinVersionString, Is.EqualTo("Windows 10 v1703 - Redstone 2"));
            Assert.That(WinVersion.Windows10_1709.WinVersionString, Is.EqualTo("Windows 10 v1709 - Redstone 3"));
            Assert.That(WinVersion.Windows10_1803.WinVersionString, Is.EqualTo("Windows 10 v1803 - Redstone 4"));
            Assert.That(WinVersion.Windows10_1809.WinVersionString, Is.EqualTo("Windows 10 v1809 - Redstone 5"));
            Assert.That(WinVersion.Windows10_1903.WinVersionString, Is.EqualTo("Windows 10 v1903"));
            Assert.That(WinVersion.Windows10_1909.WinVersionString, Is.EqualTo("Windows 10 v1909"));
            Assert.That(WinVersion.Windows10_2004.WinVersionString, Is.EqualTo("Windows 10 v2004"));
            Assert.That(WinVersion.Windows10_20H2.WinVersionString, Is.EqualTo("Windows 10 v20H2"));
            Assert.That(WinVersion.Windows10_21H1.WinVersionString, Is.EqualTo("Windows 10 v21H1"));
            Assert.That(WinVersion.Win2016.WinVersionString, Is.EqualTo("Windows 10 Server 2016"));
            Assert.That(WinVersion.Win2019.WinVersionString, Is.EqualTo("Windows 10 Server 2019"));
        }

        [Test]
        public void WinVersionCheckWindows98SE()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.Win9x,
                MajorVersion = 4,
                MinorVersion = 10,
                BuildNumber = 2222,
                CSDVersion = " A ",
                SuiteFlags = 0,
                ProductType = 0,
                ProductInfo = 0,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = false,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win98SE, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindowsME()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.Win9x,
                MajorVersion = 4,
                MinorVersion = 90,
                BuildNumber = 3000,
                CSDVersion = "",
                SuiteFlags = 0,
                ProductType = 0,
                ProductInfo = 0,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = false,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.WinME, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindowsNT4SP6()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 4,
                MinorVersion = 0,
                BuildNumber = 1381,
                CSDVersion = "Service Pack 6",
                SuiteFlags = 0,
                ProductType = WinProductType.Workstation,
                ProductInfo = 0,
                ServicePackMajor = 6,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.WinNT4, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows2000()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 0,
                BuildNumber = 2195,
                CSDVersion = "",
                SuiteFlags = 0,
                ProductType = WinProductType.Workstation,
                ProductInfo = 0,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2000, Is.EqualTo(winver));
            Assert.That(winver.ProductTypeString, Is.EqualTo("Professional"));
            Assert.That(winver.SuiteString, Is.EqualTo(""));

            winver.SuiteFlags = WinSuite.Enterprise;
            Assert.That(winver.SuiteString, Is.EqualTo("Advanced"));
        }

        [Test]
        public void WinVersionCheckWindows2000SP3()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 0,
                BuildNumber = 2195,
                CSDVersion = "Service Pack 3",
                SuiteFlags = 0,
                ProductType = WinProductType.Workstation,
                ProductInfo = 0,
                ServicePackMajor = 3,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2000, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindowsXPHome()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 1,
                BuildNumber = 2600,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x300,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Home_Premium,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.WinXP, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindowsXPHomeSP2()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 1,
                BuildNumber = 2600,
                CSDVersion = "Service Pack 2",
                SuiteFlags = (WinSuite)0x300,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Home_Premium,
                ServicePackMajor = 2,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.WinXP, Is.EqualTo(winver));
            Assert.That(WinVersion.WinXPSP2, Is.LessThanOrEqualTo(winver));
            Assert.That(WinVersion.WinXPSP3, Is.GreaterThan(winver));
        }

        [Test]
        public void WinVersionCheckWindowsXPSP3()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 1,
                BuildNumber = 2600,
                CSDVersion = "Service Pack 3",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 3,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.WinXP, Is.EqualTo(winver));
            Assert.That(WinVersion.WinXPSP2, Is.LessThanOrEqualTo(winver));
            Assert.That(WinVersion.WinXPSP3, Is.LessThanOrEqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindowsXPx64()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 2,
                BuildNumber = 3790,
                CSDVersion = "Service Pack 1",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 1,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.WinXPx64, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows2003()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 5,
                MinorVersion = 2,
                BuildNumber = 3790,
                CSDVersion = "Service Pack 2",
                SuiteFlags = (WinSuite)0x8131,
                ProductType = WinProductType.Server,
                ProductInfo = 0,
                ServicePackMajor = 2,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2003, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindowsVista()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 0,
                BuildNumber = 6002,
                CSDVersion = "Service Pack 2",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Ultimate,
                ServicePackMajor = 2,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Vista, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows2008()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 0,
                BuildNumber = 6002,
                CSDVersion = "Service Pack 2",
                SuiteFlags = (WinSuite)0x112,
                ProductType = WinProductType.Server,
                ProductInfo = WinProductInfo.Enterprise_Server,
                ServicePackMajor = 2,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2008, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows7()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 1,
                BuildNumber = 7601,
                CSDVersion = "Service Pack 1",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Ultimate,
                ServicePackMajor = 1,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win7, Is.EqualTo(winver));

            winver.SuiteFlags |= WinSuite.Enterprise;
            Assert.That(winver.SuiteString, Is.EqualTo("Enterprise"));
        }

        [Test]
        public void WinVersionCheckWindows7x64()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 1,
                BuildNumber = 7601,
                CSDVersion = "Service Pack 1",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Ultimate,
                ServicePackMajor = 1,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win7, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows2008R2DC()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 1,
                BuildNumber = 7601,
                CSDVersion = "Service Pack 1",
                SuiteFlags = (WinSuite)0x112,
                ProductType = WinProductType.DomainController,
                ProductInfo = WinProductInfo.Enterprise_Server,
                ServicePackMajor = 1,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2008R2, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows2008R2()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 1,
                BuildNumber = 7601,
                CSDVersion = "Service Pack 1",
                SuiteFlags = (WinSuite)0x112,
                ProductType = WinProductType.Server,
                ProductInfo = WinProductInfo.Enterprise_Server,
                ServicePackMajor = 1,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2008R2, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows8x64()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 2,
                BuildNumber = 9200,
                CSDVersion = "",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win8, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows8_1x64()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 3,
                BuildNumber = 9431,
                CSDVersion = "",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win8_1, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows2012R2()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 3,
                BuildNumber = 9431,
                CSDVersion = "",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Server,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                IsExtendedPropsSet = true,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2012R2, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows10_20H2()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 19042,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Enterprise,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Windows10_20H2, Is.EqualTo(winver));
        }

        [Test]
        public void WinVersionCheckWindows10Server2019DC()
        {
            WinVersion winver = new WinVersion {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 17763,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x110,
                ProductType = WinProductType.DomainController,
                ProductInfo = WinProductInfo.Standard_Server,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(WinVersion.Win2019, Is.EqualTo(winver));
            Assert.That(winver.IsServer, Is.True);
        }
    }
}
