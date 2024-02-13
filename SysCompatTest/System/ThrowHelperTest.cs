namespace System
{
    using NUnit.Framework;

    [TestFixture]
    public class ThrowHelperTest
    {
        private string m_TestString;

        private string TestString
        {
            get { return m_TestString; }
            set
            {
                ThrowHelper.ThrowIfNull(value);
                m_TestString = value;
            }
        }

        [Test]
        public void ThrowIfNull()
        {
            Assert.That(() => {
                string myArg = null;
                ThrowHelper.ThrowIfNull(myArg);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("myArg"));
        }

        [Test]
        public void ThrowPropertyIfNull()
        {
            Assert.That(TestString, Is.Null);
            Assert.That(() => {
                TestString = null;
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("value"));
        }
    }
}
