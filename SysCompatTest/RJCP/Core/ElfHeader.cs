namespace RJCP.Core
{
    using System.Runtime.InteropServices;

    internal static class ElfHeader
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Elf64SHdr
        {
            public uint sh_name;
            public uint sh_type;
            public ulong sh_flags;
            public ulong sh_addr;
            public ulong sh_offset;
            public ulong sh_size;
            public uint sh_link;
            public uint sh_info;
            public ulong sh_addralign;
            public ulong sh_entsize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Elf32SHdr
        {
            public uint p_type;
            public uint p_offset;
            public uint p_vaddr;
            public uint p_paddr;
            public uint p_filesz;
            public uint p_memsz;
            public uint p_flags;
            public uint p_align;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Elf64Hdr
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] e_ident;
            public ushort e_type;
            public ushort e_machine;
            public uint e_version;
            public ulong e_entry;
            public ulong e_phoff;
            public ulong e_shoff;
            public uint e_flags;
            public ushort e_ehsize;
            public ushort e_phentsize;
            public ushort e_phnum;
            public ushort e_shentsize;
            public ushort e_shnum;
            public ushort e_shstrndx;
        }
    }
}
