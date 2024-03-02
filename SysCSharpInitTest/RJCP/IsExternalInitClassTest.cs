namespace RJCP
{
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
