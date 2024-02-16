namespace RJCP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class IsExternalInitClassTest
    {
        [Test]
        public void TestInitClass()
        {
            IsExternalInitClass c = new IsExternalInitClass() {
                InitValue = "Value"
            };

            Assert.That(c.InitValue, Is.EqualTo("Value"));
        }
    }
}
