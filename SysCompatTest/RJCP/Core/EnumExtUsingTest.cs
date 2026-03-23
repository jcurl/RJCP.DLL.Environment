namespace RJCP.Core
{
    using System;
    using System.Linq;
    using NUnit.Framework;

#if NETFRAMEWORK
    using Enum = RJCP.Core.EnumExt;
#endif

    [TestFixture]
    public class EnumExtUsingTest
    {
        [Test]
        public void IsDefined()
        {
            // Shsws how we can use
            // <c>using Enum = RJCP.Core.EnumExt;</c>
            // for a direct replacement of
            // <c>System.Enum</c>
            Assert.That(Enum.IsDefined(ConsoleColor.Red), Is.True);
            Assert.That(Enum.IsDefined((ConsoleColor)100), Is.False);
        }

        [Test]
        public void GetValues()
        {
            // Shsws how we can use
            // <c>using Enum = RJCP.Core.EnumExt;</c>
            // for a direct replacement of
            // <c>System.Enum</c>
#if NETFRAMEWORK
            var values = System.Enum.GetValues(typeof(ConsoleColor));
#else
            var values = System.Enum.GetValues<ConsoleColor>();
#endif
            var valuesExt = Enum.GetValues<ConsoleColor>();
            Assert.That(valuesExt, Has.Length.EqualTo(values.Length));
            Assert.That(valuesExt, Is.EquivalentTo(values.Cast<ConsoleColor>()));
        }
    }
}
