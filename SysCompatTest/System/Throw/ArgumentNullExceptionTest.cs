namespace System.Throw
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentNullExceptionTest
    {
        private string m_TestString = "TestString";

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
            const string message = "ThrowIfNull";

            Assert.That(() => {
                string myArg = null;
                ThrowHelper.ThrowIfNull(myArg);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("myArg"));

            ThrowHelper.ThrowIfNull(message);
        }

        [Test]
        public void ThrowPropertyIfNull()
        {
            string orig = TestString;
            Assert.That(() => {
                TestString = null;
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("value"));
            Assert.That(TestString, Is.EqualTo(orig));
        }

        private string m_TestStringEmpty = "TestStringEmpty";

        private string TestStringEmpty
        {
            get { return m_TestStringEmpty; }
            set
            {
                ThrowHelper.ThrowIfNullOrEmpty(value);
                m_TestStringEmpty = value;
            }
        }

        [Test]
        public void ThrowIfNullOrEmpty()
        {
            const string message = "ThrowIfNull";

            Assert.That(() => {
                string myArg = string.Empty;
                ThrowHelper.ThrowIfNullOrEmpty(myArg);
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("myArg"));

            ThrowHelper.ThrowIfNullOrEmpty(message);
        }

        [Test]
        public void ThrowIfNullOrEmpty_IsNull()
        {
            Assert.That(() => {
                string myArg = null;
                ThrowHelper.ThrowIfNullOrEmpty(myArg);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("myArg"));
        }

        [Test]
        public void ThrowIfNullOrEmptyMsg()
        {
            const string message = "ThrowIfNull";
#if NETFRAMEWORK
            string result = $"Throw if null or empty{Environment.NewLine}Parameter name: myArg";
#else
            const string result = "Throw if null or empty (Parameter 'myArg')";
#endif

            Assert.That(() => {
                string myArg = string.Empty;
                ThrowHelper.ThrowIfNullOrEmptyMsg(myArg, "Throw if null or empty");
            }, Throws.TypeOf<ArgumentException>()
                .With.Property("ParamName").EqualTo("myArg")
                .And.Message.EqualTo(result));

            ThrowHelper.ThrowIfNullOrEmptyMsg(message, "Throw if null or empty");
        }

        [Test]
        public void ThrowIfNullOrEmptyProperty()
        {
            string orig = TestStringEmpty;
            Assert.That(() => {
                TestStringEmpty = string.Empty;
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("value"));
            Assert.That(TestStringEmpty, Is.EqualTo(orig));
        }

        [Test]
        public void ThrowIfNullOrEmptyProperty_IsNull()
        {
            string orig = TestStringEmpty;
            Assert.That(() => {
                TestStringEmpty = null;
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("value"));
            Assert.That(TestStringEmpty, Is.EqualTo(orig));
        }

        private string m_TestStringWhiteSpace = "TestStringWhiteSpace";

        private string TestStringWhiteSpace
        {
            get { return m_TestStringWhiteSpace; }
            set
            {
                ThrowHelper.ThrowIfNullOrWhiteSpace(value);
                m_TestStringWhiteSpace = value;
            }
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace()
        {
            const string message = "ThrowIfNull";

            Assert.That(() => {
                string myArg = string.Empty;
                ThrowHelper.ThrowIfNullOrWhiteSpace(myArg);
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("myArg"));

            ThrowHelper.ThrowIfNullOrWhiteSpace(message);
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_Whitespace()
        {
            Assert.That(() => {
                string myArg = "     ";
                ThrowHelper.ThrowIfNullOrWhiteSpace(myArg);
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("myArg"));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_IsNull()
        {
            Assert.That(() => {
                string myArg = null;
                ThrowHelper.ThrowIfNullOrWhiteSpace(myArg);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("myArg"));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpaceMsg()
        {
            const string message = "ThrowIfNull";
#if NETFRAMEWORK
            string result = $"Throw if null or empty{Environment.NewLine}Parameter name: myArg";
#else
            const string result = "Throw if null or empty (Parameter 'myArg')";
#endif

            Assert.That(() => {
                string myArg = string.Empty;
                ThrowHelper.ThrowIfNullOrWhiteSpaceMsg(myArg, "Throw if null or empty");
            }, Throws.TypeOf<ArgumentException>()
                .With.Property("ParamName").EqualTo("myArg")
                .And.Message.EqualTo(result));

            ThrowHelper.ThrowIfNullOrWhiteSpaceMsg(message, "Throw if null or empty");
        }

        [Test]
        public void ThrowIfNullOrWhiteSpaceMsg_WhiteSpace()
        {
            const string message = "ThrowIfNull";
#if NETFRAMEWORK
            string result = $"Throw if null or whitespace{Environment.NewLine}Parameter name: myArg";
#else
            const string result = "Throw if null or whitespace (Parameter 'myArg')";
#endif

            Assert.That(() => {
                string myArg = "     ";
                ThrowHelper.ThrowIfNullOrWhiteSpaceMsg(myArg, "Throw if null or whitespace");
            }, Throws.TypeOf<ArgumentException>()
                .With.Property("ParamName").EqualTo("myArg")
                .And.Message.EqualTo(result));

            ThrowHelper.ThrowIfNullOrWhiteSpaceMsg(message, "Throw if null or whitespace");
        }

        [Test]
        public void ThrowIfNullOrWhiteSpaceProperty()
        {
            string orig = TestStringWhiteSpace;
            Assert.That(() => {
                TestStringWhiteSpace = "  ";
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("value"));
            Assert.That(TestStringWhiteSpace, Is.EqualTo(orig));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpaceProperty_IsNull()
        {
            string orig = TestStringWhiteSpace;
            Assert.That(() => {
                TestStringWhiteSpace = null;
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("value"));
            Assert.That(TestStringWhiteSpace, Is.EqualTo(orig));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpaceProperty_IsEmpty()
        {
            string orig = TestStringWhiteSpace;
            Assert.That(() => {
                TestStringWhiteSpace = string.Empty;
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("value"));
            Assert.That(TestStringWhiteSpace, Is.EqualTo(orig));
        }
    }
}
