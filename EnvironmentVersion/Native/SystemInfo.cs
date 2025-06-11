namespace RJCP.Core.Environment.Native
{
    internal class SystemInfo
    {
        public uint dwOemId;
        public ushort wProcessorArchitecture;
        public uint dwPageSize;
        public ulong lpMinimumApplicationAddress;
        public ulong lpMaximumApplicationAddress;
        public ulong dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort wProcessorLevel;
        public ushort wProcessorRevision;
    }
}
