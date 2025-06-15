namespace RJCP.Core.Environment.Native
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml;

    internal class XmlWinVersion : INativeWinVersion
    {
        private struct IsWow64Process2Result
        {
            public ushort ProcessMachine;
            public ushort NativeMachine;
        }

        private sealed class ProductInfoVersion
        {
            private readonly uint m_OsMajor;
            private readonly uint m_OsMinor;
            private readonly uint m_SpMajor;
            private readonly uint m_SpMinor;

            public ProductInfoVersion(uint osMajor, uint osMinor, uint spMajor, uint spMinor)
            {
                m_OsMajor = osMajor;
                m_OsMinor = osMinor;
                m_SpMajor = spMajor;
                m_SpMinor = spMinor;
            }

            public override int GetHashCode()
            {
                return
                    m_OsMajor.GetHashCode() ^
                    m_OsMinor.GetHashCode() ^
                    m_SpMajor.GetHashCode() ^
                    m_SpMinor.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                ProductInfoVersion other = obj as ProductInfoVersion;
                return
                    other.m_OsMajor == m_OsMajor &&
                    other.m_OsMinor == m_OsMinor &&
                    other.m_SpMajor == m_SpMajor &&
                    other.m_SpMinor == m_SpMinor;
            }
        }

        private readonly bool m_HasNativeSystemInfo;
        private readonly SystemInfo m_NativeSystemInfo;

        private readonly bool m_HasSystemInfo;
        private readonly SystemInfo m_SystemInfo;

        private readonly bool m_HasGetVersion;
        private readonly uint m_GetVersion;

        private readonly bool m_HasGetVersionExSmall;
        private readonly OsVersionInfo m_GetVersionExSmall;

        private readonly bool m_HasGetVersionEx;
        private readonly OsVersionInfoEx m_GetVersionEx;

        private readonly bool m_HasRtlGetVersionSmall;
        private readonly OsVersionInfo m_RtlGetVersionSmall;

        private readonly bool m_HasRtlGetVersion;
        private readonly OsVersionInfoEx m_RtlGetVersion;

        private readonly bool m_IsWow64Process;
        private readonly bool m_HasIsWow64Process;

        private readonly IsWow64Process2Result m_IsWow64Process2;
        private readonly bool m_HasIsWow64Process2;

        private readonly Dictionary<ProductInfoVersion, uint> m_ProductInfo = new();

        private readonly Dictionary<int, int> m_SystemMetrics = new();

        private readonly Dictionary<string, string> m_Branding = new();

        private readonly Dictionary<string, Dictionary<string, RegistryKeyValue>> m_HKLM = new(StringComparer.OrdinalIgnoreCase);

        private static void RaiseException(string message, XmlNode node)
        {
            RaiseException(message, null, node);
        }

        private static void RaiseException(string message, Exception inner, XmlNode node)
        {
            IXmlLineInfo lineInfo = node as IXmlLineInfo;
            if (lineInfo is not null) {
                throw new XmlException(message, inner, lineInfo.LineNumber, lineInfo.LinePosition);
            }
            throw new XmlException(message);
        }

        private static string ChildNode(XmlNode node, string field)
        {
            var x = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(node =>
                node.Name == "Field" &&
                node.Attributes is not null &&
                string.Compare(node.Attributes["name"].Value, field, StringComparison.InvariantCulture) == 0);
            return x?.InnerText;
        }

        private static SystemInfo GetSystemInfo(XmlNode node)
        {
            if (node is null) return null;
            SystemInfo sysInfo = new() {
                dwOemId = uint.Parse(ChildNode(node, "dwOemId")),
                wProcessorArchitecture = ushort.Parse(ChildNode(node, "wProcessorArchitecture")),
                dwPageSize = uint.Parse(ChildNode(node, "dwPageSize")),
                lpMinimumApplicationAddress = ulong.Parse(ChildNode(node, "lpMinimumApplicationAddress"), NumberStyles.HexNumber),
                lpMaximumApplicationAddress = ulong.Parse(ChildNode(node, "lpMaximumApplicationAddress"), NumberStyles.HexNumber),
                dwActiveProcessorMask = ulong.Parse(ChildNode(node, "dwActiveProcessorMask")),
                dwNumberOfProcessors = uint.Parse(ChildNode(node, "dwNumberOfProcessors")),
                dwProcessorType = uint.Parse(ChildNode(node, "dwProcessorType")),
                dwAllocationGranularity = uint.Parse(ChildNode(node, "dwAllocationGranularity")),
                wProcessorLevel = ushort.Parse(ChildNode(node, "wProcessorLevel")),
                wProcessorRevision = ushort.Parse(ChildNode(node, "wProcessorRevision"))
            };
            return sysInfo;
        }

        private static OsVersionInfo GetOsVersionInfoExSmall(XmlNode node)
        {
            if (node is null) return null;
            OsVersionInfo osInfo = new() {
                MajorVersion = uint.Parse(ChildNode(node, "dwMajorVersion")),
                MinorVersion = uint.Parse(ChildNode(node, "dwMinorVersion")),
                BuildNumber = uint.Parse(ChildNode(node, "dwBuildNumber")),
                PlatformId = uint.Parse(ChildNode(node, "dwPlatformId")),
                CSDVersion = ChildNode(node, "szCSDVersion")
            };
            return osInfo;
        }

        private static OsVersionInfoEx GetOsVersionInfoEx(XmlNode node)
        {
            if (node is null) return null;
            OsVersionInfoEx osInfo = new() {
                MajorVersion = uint.Parse(ChildNode(node, "dwMajorVersion")),
                MinorVersion = uint.Parse(ChildNode(node, "dwMinorVersion")),
                BuildNumber = uint.Parse(ChildNode(node, "dwBuildNumber")),
                PlatformId = uint.Parse(ChildNode(node, "dwPlatformId")),
                CSDVersion = ChildNode(node, "szCSDVersion"),
                ServicePackMajor = ushort.Parse(ChildNode(node, "wServicePackMajor")),
                ServicePackMinor = ushort.Parse(ChildNode(node, "wServicePackMinor")),
                SuiteMask = ushort.Parse(ChildNode(node, "wSuiteMask")),
                ProductType = byte.Parse(ChildNode(node, "wProductType"))
            };
            return osInfo;
        }

        public XmlWinVersion(XmlNode winDocNode)
        {
            ThrowHelper.ThrowIfNull(winDocNode);
            if (!winDocNode.Name.Equals("WinVersionQuery"))
                RaiseException($"Invalid node, expected 'WinVersionQuery', got '{winDocNode.Name}'", winDocNode);

            XmlNode xmlGetNativeSystemInfo = winDocNode.SelectSingleNode("/WinVersionQuery/API/GetNativeSystemInfo");
            try {
                m_NativeSystemInfo = GetSystemInfo(xmlGetNativeSystemInfo);
                m_HasNativeSystemInfo = (m_NativeSystemInfo is not null);
            } catch (Exception ex) {
                RaiseException("Error parsing 'GetNativeSystemInfo'", ex, xmlGetNativeSystemInfo);
            }

            XmlNode xmlGetSystemInfo = winDocNode.SelectSingleNode("/WinVersionQuery/API/GetSystemInfo");
            try {
                m_SystemInfo = GetSystemInfo(xmlGetSystemInfo);
                m_HasSystemInfo = (m_SystemInfo is not null);
            } catch (Exception ex) {
                RaiseException("Error parsing 'GetSystemInfo'", ex, xmlGetSystemInfo);
            }

            XmlNode xmlGetVersion = winDocNode.SelectSingleNode("/WinVersionQuery/API/GetVersion");
            if (xmlGetVersion is not null) {
                try {
                    m_GetVersion = uint.Parse(xmlGetVersion.Attributes["return"].Value);
                    m_HasGetVersion = true;
                } catch (Exception ex) {
                    RaiseException("Error parsing 'GetVersion'", ex, xmlGetVersion);
                }
            }

            XmlNode xmlGetVersionExSmall = winDocNode.SelectSingleNode("/WinVersionQuery/API/GetVersionEx/Field[@name='dwOSVersionInfoSize'][text() = '276']/parent::*");
            if (xmlGetVersionExSmall is not null) {
                try {
                    m_GetVersionExSmall = GetOsVersionInfoExSmall(xmlGetVersionExSmall);
                    m_HasGetVersionExSmall = m_GetVersionExSmall is not null;
                } catch (Exception ex) {
                    RaiseException("Error parsing 'GetVersionEx'", ex, xmlGetVersionExSmall);
                }
            }

            XmlNode xmlGetVersionEx = winDocNode.SelectSingleNode("/WinVersionQuery/API/GetVersionEx/Field[@name='dwOSVersionInfoSize'][text() = '284']/parent::*");
            if (xmlGetVersionEx is not null) {
                try {
                    m_GetVersionEx = GetOsVersionInfoEx(xmlGetVersionEx);
                    m_HasGetVersionEx = m_GetVersionEx is not null;
                } catch (Exception ex) {
                    RaiseException("Error parsing 'GetVersionEx'", ex, xmlGetVersionEx);
                }
            }

            XmlNode xmlRtlGetVersion = winDocNode.SelectSingleNode("/WinVersionQuery/API/RtlGetVersion/Field[@name='dwOSVersionInfoSize'][text() = '284']/parent::*");
            if (xmlRtlGetVersion is not null) {
                try {
                    m_RtlGetVersion = GetOsVersionInfoEx(xmlRtlGetVersion);
                    m_HasRtlGetVersion = m_RtlGetVersion is not null;
                    m_RtlGetVersionSmall = GetOsVersionInfoExSmall(xmlRtlGetVersion);
                    m_HasRtlGetVersionSmall = m_RtlGetVersionSmall is not null;
                } catch (Exception ex) {
                    RaiseException("Error parsing 'RtlGetVersion'", ex, xmlRtlGetVersion);
                }
            }

            XmlNode xmlIsWow64Process = winDocNode.SelectSingleNode("/WinVersionQuery/API/IsWow64Process");
            if (xmlIsWow64Process is not null) {
                try {
                    m_IsWow64Process = uint.Parse(xmlIsWow64Process.Attributes["Wow64Process"].Value) != 0;
                    m_HasIsWow64Process = true;
                } catch (Exception ex) {
                    RaiseException("Error parsing 'IsWow64Process'", ex, xmlIsWow64Process);
                }
            }

            XmlNode xmlIsWow64Process2 = winDocNode.SelectSingleNode("/WinVersionQuery/API/IsWow64Process2");
            if (xmlIsWow64Process2 is not null) {
                try {
                    m_IsWow64Process2.NativeMachine = ushort.Parse(ChildNode(xmlIsWow64Process2, "NativeMachine"));
                    m_IsWow64Process2.ProcessMachine = ushort.Parse(ChildNode(xmlIsWow64Process2, "ProcessMachine"));
                    m_HasIsWow64Process2 = true;
                } catch (Exception ex) {
                    RaiseException("Error parsing 'IsWow64Process2'", ex, xmlIsWow64Process2);
                }
            }

            XmlNodeList xmlProductInfos = winDocNode.SelectNodes("/WinVersionQuery/API/GetProductInfo");
            if (xmlProductInfos is not null && xmlProductInfos.Count > 0) {
                foreach (XmlNode xmlProductInfo in xmlProductInfos) {
                    try {
                        ProductInfoVersion version = new(
                            uint.Parse(xmlProductInfo.Attributes["osMajor"].Value),
                            uint.Parse(xmlProductInfo.Attributes["osMinor"].Value),
                            uint.Parse(xmlProductInfo.Attributes["spMajor"].Value),
                            uint.Parse(xmlProductInfo.Attributes["spMinor"].Value)
                        );
                        uint productType = uint.Parse(ChildNode(xmlProductInfo, "ProductType"));
#if !NETFRAMEWORK
                        m_ProductInfo.TryAdd(version, productType);
#else
                        if (!m_ProductInfo.ContainsKey(version)) {
                            m_ProductInfo.Add(version, productType);
                        }
#endif
                    } catch (Exception ex) {
                        RaiseException("Error parsing 'GetProductInfo'", ex, xmlProductInfo);
                    }
                }
            }

            XmlNodeList xmlSystemMetrics = winDocNode.SelectNodes("/WinVersionQuery/API/GetSystemMetrics");
            if (xmlSystemMetrics is not null && xmlSystemMetrics.Count > 0) {
                foreach (XmlNode xmlSystemMetric in xmlSystemMetrics) {
                    try {
                        int index = int.Parse(xmlSystemMetric.Attributes["nIndex"].Value);
                        int result = int.Parse(xmlSystemMetric.Attributes["result"].Value);
#if !NETFRAMEWORK
                        m_SystemMetrics.TryAdd(index, result);
#else
                        if (!m_SystemMetrics.ContainsKey(index)) {
                            m_SystemMetrics.Add(index, result);
                        }
#endif
                    } catch (Exception ex) {
                        RaiseException("Error parsing 'GetSystemMetrics'", ex, xmlSystemMetric);
                    }
                }
            }

            XmlNodeList xmlBrandingStrings = winDocNode.SelectNodes("/WinVersionQuery/API/BrandingFormatString");
            if (xmlBrandingStrings is not null && xmlBrandingStrings.Count > 0) {
                foreach (XmlNode xmlBrandingString in xmlBrandingStrings) {
                    try {
                        string format = xmlBrandingString.Attributes["format"].Value;
                        string result = xmlBrandingString.InnerText;
#if !NETFRAMEWORK
                        m_Branding.TryAdd(format, result);
#else
                        if (!m_Branding.ContainsKey(format)) {
                            m_Branding.Add(format, result);
                        }
#endif
                    } catch (Exception ex) {
                        RaiseException("Error parsing 'BrandingFormatString'", ex, xmlBrandingString);
                    }
                }
            }

            XmlNodeList xmlHKLMRegistry = winDocNode.SelectNodes("/WinVersionQuery/API/Registry[@hive='HKLM']");
            if (xmlHKLMRegistry is not null && xmlHKLMRegistry.Count > 0) {
                foreach (XmlNode xmlHKLMKeyPath in xmlHKLMRegistry) {
                    try {
                        string path = xmlHKLMKeyPath.Attributes["key"].Value;
                        if (!m_HKLM.TryGetValue(path, out Dictionary<string, RegistryKeyValue> key)) {
                            key = new(StringComparer.OrdinalIgnoreCase);
                            m_HKLM.Add(path, key);
                        }
                        XmlNodeList xmlHKLMKeys = xmlHKLMKeyPath.SelectNodes("./Value");
                        if (xmlHKLMKeys is not null && xmlHKLMKeys.Count > 0) {
                            foreach (XmlNode xmlHKLMKey in xmlHKLMKeys) {
                                string name = xmlHKLMKey.Attributes["name"].Value;
                                string type = xmlHKLMKey.Attributes["type"].Value;
                                string value = xmlHKLMKey.InnerText;
                                RegistryKeyValue reg = new(type, value);

#if !NETFRAMEWORK
                                key.TryAdd(name, reg);
#else
                                if (!key.ContainsKey(name)) {
                                    key.Add(name, reg);
                                }
#endif
                            }
                        }
                    } catch (Exception ex) {
                        RaiseException("Error parsing 'Registry'", ex, xmlHKLMKeyPath);
                    }
                }
            }
        }

        public void GetNativeSystemInfo(out SystemInfo systemInfo)
        {
            if (!m_HasNativeSystemInfo) throw new EntryPointNotFoundException();
            systemInfo = m_NativeSystemInfo;
        }

        public void GetSystemInfo(out SystemInfo systemInfo)
        {
            if (!m_HasSystemInfo) throw new EntryPointNotFoundException();
            systemInfo = m_SystemInfo;
        }

        public uint GetVersion()
        {
            if (!m_HasGetVersion) throw new EntryPointNotFoundException();
            return m_GetVersion;
        }

        public bool GetVersionEx(out OsVersionInfo osVersionInfo)
        {
            if (!m_HasGetVersionExSmall) throw new EntryPointNotFoundException();
            osVersionInfo = m_GetVersionExSmall;
            return true;
        }

        public bool GetVersionEx(out OsVersionInfoEx osVersionInfoEx)
        {
            if (!m_HasGetVersionEx) {
                osVersionInfoEx = default;
                return false;
            }
            osVersionInfoEx = m_GetVersionEx;
            return true;
        }

        public int RtlGetVersion(out OsVersionInfo osVersionInfo)
        {
            if (!m_HasRtlGetVersionSmall) throw new EntryPointNotFoundException();
            osVersionInfo = m_RtlGetVersionSmall;
            return 0;
        }

        public int RtlGetVersion(out OsVersionInfoEx osVersionInfoEx)
        {
            if (!m_HasRtlGetVersion) throw new EntryPointNotFoundException();
            osVersionInfoEx = m_RtlGetVersion;
            return 0;
        }

        public bool IsWow64Process(out bool wow64)
        {
            if (!m_HasIsWow64Process) throw new EntryPointNotFoundException();
            wow64 = m_IsWow64Process;
            return true;
        }

        public bool IsWow64Process2(out ushort processMachine, out ushort nativeMachine)
        {
            if (!m_HasIsWow64Process2) throw new EntryPointNotFoundException();
            processMachine = m_IsWow64Process2.ProcessMachine;
            nativeMachine = m_IsWow64Process2.NativeMachine;
            return true;
        }

        public bool GetProductInfo(int osMajor, int osMinor, int spMajor, int spMinor, out uint productInfo)
        {
            if (m_ProductInfo.Count == 0) throw new EntryPointNotFoundException();
            ProductInfoVersion version = new(
                unchecked((uint)osMajor),
                unchecked((uint)osMinor),
                unchecked((uint)spMajor),
                unchecked((uint)spMinor)
            );
            if (m_ProductInfo.TryGetValue(version, out productInfo)) return true;
            productInfo = unchecked((uint)-1);
            return false;
        }

        public int GetSystemMetrics(int nIndex)
        {
            if (m_SystemMetrics.Count == 0) throw new EntryPointNotFoundException();
            if (m_SystemMetrics.TryGetValue(nIndex, out int result)) return result;
            return 0;
        }

        public string BrandingFormatString(string format)
        {
            if (m_Branding.Count == 0) throw new EntryPointNotFoundException();
            if (m_Branding.TryGetValue(format, out string result)) return result;
            return string.Empty;
        }

        public IRegistryKey OpenSubKey(string hive, string path)
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(hive);
            ThrowHelper.ThrowIfNullOrWhiteSpace(path);

            if (string.Compare(hive, "HKLM", StringComparison.InvariantCultureIgnoreCase) == 0) {
                if (!m_HKLM.TryGetValue(path, out Dictionary<string, RegistryKeyValue> keys)) {
                    return null;
                }
                return new XmlRegistryKey(keys);
            } else {
                throw new NotSupportedException("Only HKLM supported");
            }
        }
    }
}
