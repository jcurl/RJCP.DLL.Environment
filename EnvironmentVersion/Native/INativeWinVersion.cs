namespace RJCP.Core.Environment.Native
{
    internal interface INativeWinVersion
    {
        public void GetNativeSystemInfo(out SystemInfo systemInfo);

        public void GetSystemInfo(out SystemInfo systemInfo);

        public uint GetVersion();

        public bool GetVersionEx(out OsVersionInfo osVersionInfo);

        public bool GetVersionEx(out OsVersionInfoEx osVersionInfoEx);

        public int RtlGetVersion(out OsVersionInfo osVersionInfo);

        public int RtlGetVersion(out OsVersionInfoEx osVersionInfoEx);

        public bool IsWow64Process(out bool wow64);

        public bool IsWow64Process2(out ushort processMachine, out ushort nativeMachine);

        public bool GetProductInfo(int osMajor, int osMinor, int spMajor, int spMinor, out uint productInfo);

        public int GetSystemMetrics(int nIndex);

        public string BrandingFormatString(string format);

        public IRegistryKey OpenSubKey(string hive, string path);
    }
}
