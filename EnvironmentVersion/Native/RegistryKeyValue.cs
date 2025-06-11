namespace RJCP.Core.Environment.Native
{
    using System;

    internal class RegistryKeyValue
    {
        private readonly string m_Type;
        private readonly string m_Value;

        public RegistryKeyValue(string type, string value)
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(type);
            ThrowHelper.ThrowIfNull(value);

            m_Type = type;
            m_Value = value;
        }

        public string Type { get { return m_Type; } }

        public string Value { get { return m_Value; } }

        public object GetValue()
        {
            if (m_Type.Equals("REG_SZ", StringComparison.InvariantCultureIgnoreCase)) {
                return m_Value;
            } else if (m_Type.Equals("REG_DWORD", StringComparison.InvariantCultureIgnoreCase)) {
                return unchecked((int)uint.Parse(m_Value));
            } else if (m_Type.Equals("REG_QWORD", StringComparison.InvariantCultureIgnoreCase)) {
                return unchecked((long)ulong.Parse(m_Value));
            } else {
                throw new NotSupportedException($"Simulated registry key type {m_Type} is not supported");
            }
        }
    }
}
