namespace RJCP.Core.Environment.Native
{
    using System;
    using System.Collections.Generic;

    internal sealed class XmlRegistryKey : IRegistryKey
    {
        private readonly Dictionary<string, RegistryKeyValue> m_Keys;

        public XmlRegistryKey(Dictionary<string, RegistryKeyValue> keys)
        {
            ThrowHelper.ThrowIfNull(keys);
            m_Keys = keys;
        }

        public object GetValue(string key)
        {
            if (!m_Keys.TryGetValue(key, out RegistryKeyValue value))
                return null;

            return value.GetValue();
        }

        public void Dispose() { }
    }
}
