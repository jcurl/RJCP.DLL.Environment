namespace RJCP.Core.Environment.Native
{
    using System;
    using System.Runtime.Versioning;
    using Microsoft.Win32;

    [SupportedOSPlatform("windows")]
    internal sealed class NativeRegistryKey : IRegistryKey
    {
        private readonly RegistryKey m_Key;

        public NativeRegistryKey(RegistryKey rk)
        {
            ThrowHelper.ThrowIfNull(rk);
            m_Key = rk;
        }

        public object GetValue(string key)
        {
            return m_Key.GetValue(key);
        }

        public void Dispose()
        {
            m_Key.Dispose();
        }
    }
}
