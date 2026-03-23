namespace RJCP.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using NUnit.Framework;

    [TestFixture]
    public class MarshalExtText
    {
        [Test]
        [SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known", Justification = "Test Case Comparison")]
        public void SizeOf_p64()
        {
            int e64 = Marshal.SizeOf(typeof(ElfHeader.Elf64SHdr));
            Assert.That(MarshalExt.SizeOf<ElfHeader.Elf64SHdr>(), Is.EqualTo(e64));
        }

        [Test]
        [SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known", Justification = "Test Case Comparison")]
        public void SizeOf_h64()
        {
            int e64 = Marshal.SizeOf(typeof(ElfHeader.Elf64Hdr));
            Assert.That(MarshalExt.SizeOf<ElfHeader.Elf64Hdr>(), Is.EqualTo(e64));
        }

        [Test]
        [SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known", Justification = "Test Case Comparison")]
        public void SizeOf_p32()
        {
            int e32 = Marshal.SizeOf(typeof(ElfHeader.Elf32SHdr));
            Assert.That(MarshalExt.SizeOf<ElfHeader.Elf32SHdr>(), Is.EqualTo(e32));
        }
    }
}
