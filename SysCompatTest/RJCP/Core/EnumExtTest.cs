namespace RJCP.Core
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class EnumExtTest
    {
        [Test]
        public void IsDefined()
        {
            Assert.That(ConsoleColor.Red.IsDefined(), Is.True);
            Assert.That(((ConsoleColor)100).IsDefined(), Is.False);
        }

        [Test]
        public void GetValues()
        {
#if NETFRAMEWORK
            var values = Enum.GetValues(typeof(ConsoleColor));
#else
            var values = Enum.GetValues<ConsoleColor>();
#endif
            var valuesExt = EnumExt.GetValues<ConsoleColor>();
            Assert.That(valuesExt, Has.Length.EqualTo(values.Length));
            Assert.That(valuesExt, Is.EquivalentTo(values.Cast<ConsoleColor>()));
        }
    }
}
