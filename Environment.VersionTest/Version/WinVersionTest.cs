namespace RJCP.Core.Environment.Version
{
    using System;
    using System.IO;
    using System.Runtime.Versioning;
    using NUnit.Framework;
    using RJCP.CodeQuality.Config;
    using RJCP.CodeQuality.NUnitExtensions;
#if NET6_0_OR_GREATER
    using System.Diagnostics.CodeAnalysis;
#endif

    [TestFixture]
    public class WinVersionTest
    {
        [Test]
        [Platform(Include = "Win32NT")]
        [SupportedOSPlatform("windows")]
        public void OSVersionCheckCurrentOS()
        {
            WinVersion current = WinVersion.LocalMachine;
            Console.WriteLine($"[Current]");
            Console.WriteLine($"PlatformId={current.PlatformId}");
            Console.WriteLine($"PlatformIdString={current.PlatformIdString}");
            Console.WriteLine($"MajorVersion={current.MajorVersion}");
            Console.WriteLine($"MinorVersion={current.MinorVersion}");
            Console.WriteLine($"BuildNumber={current.BuildNumber}");
            Console.WriteLine($"UpdateBuildNumber={current.UpdateBuildNumber}");
            Console.WriteLine($"CSDVersion={current.CSDVersion}");
            Console.WriteLine($"ServicePackMajor={current.ServicePackMajor}");
            Console.WriteLine($"ServicePackMinor={current.ServicePackMinor}");
            Console.WriteLine($"Version={current.Version}");
            Console.WriteLine($"VersionString={current.VersionString}");
            Console.WriteLine($"WinVersionString={current.WinVersionString}");
            Console.WriteLine($"NativeArchitecture={current.NativeArchitecture}");
            Console.WriteLine($"WinArchitecture={current.Architecture}");
            Console.WriteLine($"SuiteFlags={current.SuiteFlags:X}");
            Console.WriteLine($"SuiteString={current.SuiteString}");
            Console.WriteLine($"ProductType={current.ProductType}");
            Console.WriteLine($"ProductTypeString={current.ProductTypeString}");
            Console.WriteLine($"ProductInfo={current.ProductInfo}");
            Console.WriteLine($"ProductInfoString={current.ProductInfoString}");
            Console.WriteLine($"IsServer={current.IsServer}");
            Console.WriteLine($"ServerR2={current.ServerR2}");
            Console.WriteLine($"ToString={current}");
            Console.WriteLine("");

            Assert.Inconclusive("Please check output that it matches your computer");
        }

        [Test]
        [Platform(Include = "Unix")]
#if NET6_0_OR_GREATER
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Test Case confirming behaviour")]
#endif
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
            Assert.That(WinVersion.Windows10_21H1, Is.LessThan(WinVersion.Windows10_21H2));
            Assert.That(WinVersion.Windows10_21H2, Is.LessThan(WinVersion.Windows10_22H2));
            Assert.That(WinVersion.Windows10_22H2, Is.LessThan(WinVersion.Windows11_21H2));
            Assert.That(WinVersion.Windows11_21H2, Is.LessThan(WinVersion.Windows11_22H2));
            Assert.That(WinVersion.Windows11_22H2, Is.LessThan(WinVersion.Windows11_23H2));
            Assert.That(WinVersion.Windows11_23H2, Is.LessThan(WinVersion.Windows11_24H2));

            // Server build comparisons
            Assert.That(WinVersion.Win2000, Is.LessThan(WinVersion.Win2003));
            Assert.That(WinVersion.Win2003, Is.LessThan(WinVersion.Win2003R2));
            Assert.That(WinVersion.Win2003R2, Is.LessThan(WinVersion.Win2008));
            Assert.That(WinVersion.Win2008, Is.LessThan(WinVersion.Win2008R2));
            Assert.That(WinVersion.Win2008R2, Is.LessThan(WinVersion.Win2012));
            Assert.That(WinVersion.Win2012, Is.LessThan(WinVersion.Win2012R2));
            Assert.That(WinVersion.Win2012R2, Is.LessThan(WinVersion.Win2016));
            Assert.That(WinVersion.Win2016, Is.LessThan(WinVersion.Win2019));
            Assert.That(WinVersion.Win2019, Is.LessThan(WinVersion.Win2022));
            Assert.That(WinVersion.Win2022, Is.LessThan(WinVersion.Win2025));

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
            Assert.That(WinVersion.Win2022, Is.LessThan(WinVersion.Windows11_21H2));
            Assert.That(WinVersion.Win2025, Is.EqualTo(WinVersion.Windows11_24H2));
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
            Assert.That(WinVersion.Windows11_21H2.IsServer, Is.False);
            Assert.That(WinVersion.Windows11_22H2.IsServer, Is.False);
            Assert.That(WinVersion.Windows11_23H2.IsServer, Is.False);
            Assert.That(WinVersion.Windows11_24H2.IsServer, Is.False);

            Assert.That(WinVersion.Win2003.IsServer, Is.True);
            Assert.That(WinVersion.Win2008.IsServer, Is.True);
            Assert.That(WinVersion.Win2008R2.IsServer, Is.True);
            Assert.That(WinVersion.Win2012.IsServer, Is.True);
            Assert.That(WinVersion.Win2012R2.IsServer, Is.True);
            Assert.That(WinVersion.Win2016.IsServer, Is.True);
            Assert.That(WinVersion.Win2019.IsServer, Is.True);
            Assert.That(WinVersion.Win2022.IsServer, Is.True);
            Assert.That(WinVersion.Win2025.IsServer, Is.True);
        }

        [Test]
        public void WinVersionFuturistic()
        {
            WinVersion p65 = new(WinPlatform.WinNT, 6, 5);
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

            WinVersion p70 = new(WinPlatform.WinNT, 7, 0);
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

            WinVersion p100 = new(WinPlatform.WinNT, 10, 0);
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

            WinVersion p101 = new(WinPlatform.WinNT, 10, 1);
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
            Assert.That(WinVersion.Windows10_1507, Is.LessThan(p101));
            Assert.That(WinVersion.Windows10_21H1, Is.LessThan(p101));
            Assert.That(WinVersion.Win2019, Is.LessThan(p101));
            Assert.That(WinVersion.Windows11_21H2, Is.LessThan(p101));
            Assert.That(WinVersion.Win2022, Is.LessThan(p101));
            Assert.That(WinVersion.Windows11_22H2, Is.LessThan(p101));
            Assert.That(WinVersion.Windows11_23H2, Is.LessThan(p101));
            Assert.That(WinVersion.Windows11_24H2, Is.LessThan(p101));
            Assert.That(WinVersion.Win2025, Is.LessThan(p101));
        }

        [Test]
        public void WinVersionUnknown()
        {
            WinVersion ver61 = new(WinPlatform.WinNT, 6, 1) {
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
            Assert.That(WinVersion.Windows10.WinVersionString, Is.EqualTo("Windows NT 10.x"));
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
            Assert.That(WinVersion.Windows10_21H2.WinVersionString, Is.EqualTo("Windows 10 v21H2"));
            Assert.That(WinVersion.Windows10_22H2.WinVersionString, Is.EqualTo("Windows 10 v22H2"));
            Assert.That(WinVersion.Windows11_21H2.WinVersionString, Is.EqualTo("Windows 11 v21H2"));
            Assert.That(WinVersion.Windows11_22H2.WinVersionString, Is.EqualTo("Windows 11 v22H2"));
            Assert.That(WinVersion.Windows11_23H2.WinVersionString, Is.EqualTo("Windows 11 v23H2"));
            Assert.That(WinVersion.Windows11_24H2.WinVersionString, Is.EqualTo("Windows 11 v24H2"));
            Assert.That(WinVersion.Win2016.WinVersionString, Is.EqualTo("Windows 2016"));
            Assert.That(WinVersion.Win2019.WinVersionString, Is.EqualTo("Windows 2019"));
            Assert.That(WinVersion.Win2022.WinVersionString, Is.EqualTo("Windows 2022"));
            Assert.That(WinVersion.Win2025.WinVersionString, Is.EqualTo("Windows 2025"));
        }

        [Test]
        public void WinProductString()
        {
            int count = 0;
            foreach (WinProductInfo winProductInfo in Enum.GetValues(typeof(WinProductInfo))) {
                WinVersion winVersion = new() {
                    ProductInfo = winProductInfo
                };
                if (string.IsNullOrEmpty(winVersion.ProductInfoString) && winProductInfo != WinProductInfo.Undefined) {
                    count++;
                    Console.WriteLine($"Empty Product Info String for {winProductInfo}");
                }
            }
            Assert.That(count, Is.Zero);
        }

        [Test]
        public void WinVersionCheckWindows98SE()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win98SE));
        }

        [Test]
        public void WinVersionCheckWindowsME()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.WinME));
        }

        [Test]
        public void WinVersionCheckWindowsNT4SP6()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.WinNT4));
        }

        [Test]
        public void WinVersionCheckWindows2000()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2000));
            Assert.That(winver.ProductTypeString, Is.EqualTo("Professional"));
            Assert.That(winver.SuiteString, Is.EqualTo(""));

            winver.SuiteFlags = WinSuite.Enterprise;
            Assert.That(winver.SuiteString, Is.EqualTo("Advanced"));
        }

        [Test]
        public void WinVersionCheckWindows2000SP3()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2000));
        }

        [Test]
        public void WinVersionCheckWindowsXPHome()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.WinXP));
        }

        [Test]
        public void WinVersionCheckWindowsXPHomeSP2()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.WinXP));
            Assert.That(winver, Is.GreaterThan(WinVersion.Win2000));
            Assert.That(winver, Is.GreaterThanOrEqualTo(WinVersion.WinXPSP2));
            Assert.That(winver, Is.LessThan(WinVersion.WinXPSP3));
        }

        [Test]
        public void WinVersionCheckWindowsXPSP3()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.WinXP));
            Assert.That(winver, Is.GreaterThan(WinVersion.WinXPSP2));
            Assert.That(winver, Is.LessThanOrEqualTo(WinVersion.WinXPSP3));
        }

        [Test]
        public void WinVersionCheckWindowsXPx64()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.WinXPx64));
        }

        [Test]
        public void WinVersionCheckWindows2003()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2003));
        }

        [Test]
        public void WinVersionCheckWindowsVista()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Vista));
        }

        [Test]
        public void WinVersionCheckWindows2008()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2008));
        }

        [Test]
        public void WinVersionCheckWindows7()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x86
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win7));

            winver.SuiteFlags |= WinSuite.Enterprise;
            Assert.That(winver.SuiteString, Is.EqualTo("Enterprise"));
        }

        [Test]
        public void WinVersionCheckWindows7x64()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win7));
        }

        [Test]
        public void WinVersionCheckWindows2008R2DC()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2008R2));
        }

        [Test]
        public void WinVersionCheckWindows2008R2()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2008R2));
        }

        [Test]
        public void WinVersionCheckWindows8x64()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win8));
        }

        [Test]
        public void WinVersionCheckWindows8_1x64()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win8_1));
        }

        [Test]
        public void WinVersionCheckWindows8_1RT()
        {
            WinVersion winver = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 6,
                MinorVersion = 3,
                BuildNumber = 9600,
                CSDVersion = "",
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Core_Arm,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.ARM
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win8_1));
            Assert.That(winver.ToString(), Is.EqualTo("Windows 8.1 RT ARM (Home ARM), v6.3.9600.0"));
        }

        [Test]
        public void WinVersionCheckWindows2012R2()
        {
            WinVersion winver = new() {
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
                NativeArchitecture = WinArchitecture.x64
            };
            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Win2012R2));
        }

        [Test]
        public void WinVersionCheckWindows10_20H2()
        {
            WinVersion winver = new() {
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
            Assert.That(winver, Is.EqualTo(WinVersion.Windows10_20H2));
        }

        [Test]
        public void WinVersionCheckWindows11_21H2()
        {
            WinVersion winver = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 22000,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Enterprise,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Console.WriteLine($"{winver}");
            Assert.That(winver, Is.EqualTo(WinVersion.Windows11_21H2));
        }

        [Test]
        public void WinVersionCheckWindows10Server2019DC()
        {
            WinVersion winver = new() {
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
            Assert.That(winver, Is.EqualTo(WinVersion.Win2019));
            Assert.That(winver.IsServer, Is.True);
        }

        [Test]
        public void UnknownWindows10VeryEarly()
        {
            WinVersion winVersion = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 1,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Assert.That(winVersion.WinVersionString, Is.EqualTo("Windows NT 10.x"));
        }

        [Test]
        public void UnknownWindows10Early()
        {
            WinVersion winVersion = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 10000,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Assert.That(winVersion.WinVersionString, Is.EqualTo("Windows NT 10.x"));
        }

        [Test]
        public void UnknownWindows10()
        {
            WinVersion winVersion = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 19999,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Assert.That(winVersion.WinVersionString, Is.EqualTo("Windows 10 v22H2 or later"));
        }

        [Test]
        public void UnknownWindows11()
        {
            WinVersion winVersion = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = 27000,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x100,
                ProductType = WinProductType.Workstation,
                ProductInfo = WinProductInfo.Professional,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Assert.That(winVersion.WinVersionString, Is.EqualTo("Windows 11 v24H2 or later"));
        }

        [TestCase(22650)]
        [TestCase(22651)]
        [TestCase(22652)]
        public void UnknownWindows2022(int build)
        {
            WinVersion winVersion = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = build,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x110,
                ProductType = WinProductType.Server,
                ProductInfo = WinProductInfo.DataCenter_Server,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Assert.That(winVersion.WinVersionString, Is.EqualTo("Windows 2022 or later"));
        }

        [TestCase(17764)]
        [TestCase(20000)]
        [TestCase(19042)]
        public void UnknownWindows2019(int build)
        {
            WinVersion winVersion = new() {
                PlatformId = WinPlatform.WinNT,
                MajorVersion = 10,
                MinorVersion = 0,
                BuildNumber = build,
                CSDVersion = string.Empty,
                SuiteFlags = (WinSuite)0x110,
                ProductType = WinProductType.Server,
                ProductInfo = WinProductInfo.DataCenter_Server,
                ServicePackMajor = 0,
                ServicePackMinor = 0,
                NativeArchitecture = WinArchitecture.x64
            };

            Assert.That(winVersion.WinVersionString, Is.EqualTo("Windows 2019 or later"));
        }

        private readonly static string FilePath = Path.Combine(Deploy.TestDirectory, "TestResources", "WinVersion");

        private readonly static string[] WinVersionFiles = {
            "win95a",
            "win95-osr2",
            "win98-se",
            "win-me",
            "winnt351-server_x86",
            "winnt4-sp1_x86",
            "winnt4-sp1-server_x86",
            "winnt4-sp6_x86",
            "win2000-sp4-pro_x86",
            "ReactOS-0.4.15_x86",
            "ReactOS-0.4.15_x86_xp",
            "winxp-sp3-pro_x86",
            "winvista-sp1-ult_x86",
            "winvista-sp1-ult_x86_xp",
            "winvista-sp2-checked-ult_x86",
            "winvista-sp2-checked-ult_x86_xp",
            "winvista-sp2-ult_x64",
            "winvista-sp2-ult_x64_wow",
            "winvista-sp2-ult_x64_wow_xp",
            "winvista-sp2-ult_x64_xp",
            "win7-sp1-ent_x64",
            "win7-sp1-ent_x64_wow",
            "win7-sp1-ent_x64_wow_xp",
            "win7-sp1-ent_x64_xp",
            "win8-pro_x64",
            "win8-pro_x64_wow",
            "win8-pro_x64_wow_xp",
            "win8-pro_x64_xp",
            "win81-sp3-pro_x64",
            "win81-sp3-pro_x64_wow",
            "win81-sp3-pro_x64_wow_xp",
            "win81-sp3-pro_x64_xp",
            "win10.10240-pro_x64",
            "win10.10240-pro_x64_wow",
            "win10.10240-pro_x64_wow_xp",
            "win10.10240-pro_x64_xp",
            "win10.10586-home_x64",
            "win10.10586-home_x64_wow",
            "win10.10586-home_x64_wow_xp",
            "win10.10586-home_x64_xp",
            "win10.14393-pro_x64",
            "win10.14393-pro_x64_wow",
            "win10.14393-pro_x64_wow_xp",
            "win10.14393-pro_x64_xp",
            "win10.16299-edu_x64",
            "win10.16299-edu_x64_wow",
            "win10.16299-edu_x64_wow_xp",
            "win10.16299-edu_x64_xp",
            "win10.17134-ent_x64",
            "win10.17134-ent_x64_wow",
            "win10.17134-ent_x64_wow_xp",
            "win10.17134-ent_x64_xp",
            "win10.17763-ent_x64",
            "win10.17763-ent_x64_wow",
            "win10.17763-ent_x64_wow_xp",
            "win10.17763-ent_x64_xp",
            "win10.17763-ent_x86",
            "win10.17763-ent_x86_xp",
            "win10.18362-ent_x64",
            "win10.18362-ent_x64_wow",
            "win10.18362-ent_x64_wow_xp",
            "win10.18362-ent_x64_xp",
            "win10.19044-ent-iot_x64",
            "win10.19044-ent-iot_x64_wow",
            "win10.19044-ent-iot_x64_wow_xp",
            "win10.19044-ent-iot_x64_xp",
            "win10.19044-ent-ltsc_x64",
            "win10.19044-ent-ltsc_x64_wow",
            "win10.19044-ent-ltsc_x64_wow_xp",
            "win10.19044-ent-ltsc_x64_xp",
            "win10.19045-pro_x64",
            "win10.19045-pro_x64_wow",
            "win10.19045-pro_x64_wow_xp",
            "win10.19045-pro_x64_xp",
            "win11.22000-ent",
            "win11.22000-ent_wow",
            "win11.22000-ent_wow_xp",
            "win11.22000-ent_xp",
            "win11.22631-ent",
            "win11.22631-ent_wow",
            "win11.22631-ent_wow_xp",
            "win11.22631-ent_xp",
            "win11.26100-ent",
            "win11.26100-ent_wow",
            "win11.26100-ent_wow_xp",
            "win11.26100-ent_xp",
            "win2003-sp2_x86",
            "win2003r2-std_x86",
            "win2008-sp1-datacenter_x64",
            "win2008-sp1-datacenter_x64_wow",
            "win2008-sp1-datacenter_x64_wow_xp",
            "win2008-sp1-datacenter_x64_xp",
            "win2008r2-sp1-web_x64",
            "win2008r2-sp1-web_x64_wow",
            "win2008r2-sp1-web_x64_wow_xp",
            "win2008r2-sp1-web_x64_xp",
            "sbs2011_x64_wow_xp",
            "sbs2011_x64_xp",
            "sbs2011-sp1_x64",
            "sbs2011-sp1_x64_wow",
            "sbs2011-sp1_x64_wow_xp",
            "sbs2011-sp1_x64_xp",
            "win2012-datacenter_x64",
            "win2012-datacenter_x64_wow",
            "win2012-datacenter_x64_wow_xp",
            "win2012-datacenter_x64_xp",
            "win2012r2-std_x64",
            "win2012r2-std_x64_wow",
            "win2012r2-std_x64_wow_xp",
            "win2012r2-std_x64_xp",
            "win2016-essential_x64",
            "win2016-essential_x64_wow",
            "win2016-essential_x64_wow_xp",
            "win2016-essential_x64_xp",
            "win2016-essential_x64",
            "win2019-ad-std_x64",
            "win2019-ad-std_x64_wow",
            "win2019-ad-std_x64_wow_xp",
            "win2019-ad-std_x64_xp",
            "win2022-std_x64",
            "win2022-std_x64_wow",
            "win2022-std_x64_wow_xp",
            "win2022-std_x64_xp",
            "win2025-std",
            "win2025-std_wow",
            "win2025-std_wow_xp",
            "win2025-std_xp",
            "wine-6.0.3_x64",
            "wine-6.0.3_x64_wow",
            "wine-6.0.3_x64_wow_xp",
            "wine-6.0.3_x64_xp"
        };

        [Test]
        public void GenerateIni()
        {
            bool exception = false;

            using (ScratchPad pad = Deploy.ScratchPad(nameof(GenerateIni), ScratchOptions.UseScratchDir | ScratchOptions.CreateScratch))
            using (FileStream fs = new(Path.Combine(pad.Path, "winversion.ini"), FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new(fs)) {
                foreach (string fileName in WinVersionFiles) {
                    try {
                        WinVersion winVersion = WinVersion.Load(Path.Combine(FilePath, $"{fileName}.xml"));
                        w.WriteLine($"[{fileName}]");
                        w.WriteLine($"PlatformId={winVersion.PlatformId}");
                        w.WriteLine($"PlatformIdString={winVersion.PlatformIdString}");
                        w.WriteLine($"MajorVersion={winVersion.MajorVersion}");
                        w.WriteLine($"MinorVersion={winVersion.MinorVersion}");
                        w.WriteLine($"BuildNumber={winVersion.BuildNumber}");
                        w.WriteLine($"UpdateBuildNumber={winVersion.UpdateBuildNumber}");
                        w.WriteLine($"CSDVersion={winVersion.CSDVersion}");
                        w.WriteLine($"ServicePackMajor={winVersion.ServicePackMajor}");
                        w.WriteLine($"ServicePackMinor={winVersion.ServicePackMinor}");
                        w.WriteLine($"Version={winVersion.Version}");
                        w.WriteLine($"VersionString={winVersion.VersionString}");
                        w.WriteLine($"WinVersionString={winVersion.WinVersionString}");
                        w.WriteLine($"NativeArchitecture={winVersion.NativeArchitecture}");
                        w.WriteLine($"WinArchitecture={winVersion.Architecture}");
                        w.WriteLine($"SuiteFlags={winVersion.SuiteFlags}");
                        w.WriteLine($"SuiteString={winVersion.SuiteString}");
                        w.WriteLine($"ProductType={winVersion.ProductType}");
                        w.WriteLine($"ProductTypeString={winVersion.ProductTypeString}");
                        w.WriteLine($"ProductInfo={winVersion.ProductInfo}");
                        w.WriteLine($"ProductInfoString={winVersion.ProductInfoString}");
                        w.WriteLine($"IsServer={winVersion.IsServer}");
                        w.WriteLine($"ServerR2={winVersion.ServerR2}");
                        w.WriteLine($"ToString={winVersion}");
                        w.WriteLine("");
                    } catch (System.Xml.XmlException ex) {
                        Console.WriteLine($"Exception in file {fileName} - {ex.Message}");
                        exception = true;
                    } catch (Exception ex) {
                        Console.WriteLine($"Exception in file {fileName} - {ex}");
                        exception = true;
                    }
                }
            }

            Assert.That(exception, Is.False);
        }

        [TestCaseSource(nameof(WinVersionFiles))]
        public void WindowsVersionQueryXml(string fileName)
        {
            IniFile versionResults = new(Path.Combine(FilePath, "winversion.ini"));
            IniSection versionResult = versionResults[fileName];

            WinVersion winVersion = WinVersion.Load(Path.Combine(FilePath, $"{fileName}.xml"));

            Assert.That(winVersion.PlatformId.ToString(), Is.EqualTo(versionResult["PlatformId"]));
            Assert.That(winVersion.PlatformIdString, Is.EqualTo(versionResult["PlatformIdString"]));
            Assert.That(winVersion.MajorVersion.ToString(), Is.EqualTo(versionResult["MajorVersion"]));
            Assert.That(winVersion.MinorVersion.ToString(), Is.EqualTo(versionResult["MinorVersion"]));
            Assert.That(winVersion.BuildNumber.ToString(), Is.EqualTo(versionResult["BuildNumber"]));
            Assert.That(winVersion.UpdateBuildNumber.ToString(), Is.EqualTo(versionResult["UpdateBuildNumber"]));
            Assert.That(winVersion.CSDVersion.Trim(), Is.EqualTo(versionResult["CSDVersion"]));
            Assert.That(winVersion.ServicePackMajor.ToString(), Is.EqualTo(versionResult["ServicePackMajor"]));
            Assert.That(winVersion.ServicePackMinor.ToString(), Is.EqualTo(versionResult["ServicePackMinor"]));
            Assert.That(winVersion.Version.ToString(), Is.EqualTo(versionResult["Version"]));
            Assert.That(winVersion.VersionString, Is.EqualTo(versionResult["VersionString"]));
            Assert.That(winVersion.WinVersionString, Is.EqualTo(versionResult["WinVersionString"]));
            Assert.That(winVersion.NativeArchitecture.ToString(), Is.EqualTo(versionResult["NativeArchitecture"]));
            Assert.That(winVersion.Architecture.ToString(), Is.EqualTo(versionResult["WinArchitecture"]));
            Assert.That(winVersion.SuiteFlags.ToString(), Is.EqualTo(versionResult["SuiteFlags"]));
            Assert.That(winVersion.SuiteString, Is.EqualTo(versionResult["SuiteString"]));
            Assert.That(winVersion.ProductType.ToString(), Is.EqualTo(versionResult["ProductType"]));
            Assert.That(winVersion.ProductTypeString, Is.EqualTo(versionResult["ProductTypeString"]));
            Assert.That(winVersion.ProductInfo.ToString(), Is.EqualTo(versionResult["ProductInfo"]));
            Assert.That(winVersion.ProductInfoString, Is.EqualTo(versionResult["ProductInfoString"]));
            Assert.That(winVersion.IsServer.ToString(), Is.EqualTo(versionResult["IsServer"]));
            Assert.That(winVersion.ServerR2.ToString(), Is.EqualTo(versionResult["ServerR2"]));
            Assert.That(winVersion.ToString(), Is.EqualTo(versionResult["ToString"]));
        }
    }
}
