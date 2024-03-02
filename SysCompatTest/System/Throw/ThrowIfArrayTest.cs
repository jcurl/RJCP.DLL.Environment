namespace System.Throw
{
    using NUnit.Framework;

    [TestFixture]
    public class ThrowIfArrayTest
    {
        [Test]
        public void ThrowIfArrayEmpty_Null()
        {
            string[] array = null;

            Assert.That(() => {
                ThrowHelper.ThrowIfArrayEmpty(array);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("array"));
        }

        [Test]
        public void ThrowIfArrayEmpty()
        {
#if NET40
            string[] emptyArray = new string[0];
#else
            string[] emptyArray = Array.Empty<string>();
#endif

            Assert.That(() => {
                ThrowHelper.ThrowIfArrayEmpty(emptyArray);
            }, Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("emptyArray"));

            string[] array = new string[] { "Foo" };
            ThrowHelper.ThrowIfArrayEmpty(array);
        }

        [Test]
        public void ThrowIfArrayBounds_Null()
        {
            string[] array = null;

            Assert.That(() => {
                ThrowHelper.ThrowIfArrayOutOfBounds(array, 0);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("array"));

            Assert.That(() => {
                ThrowHelper.ThrowIfArrayOutOfBounds(array, (long)0);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("array"));
        }

        [Test]
        public void ThrowIfArrayBounds()
        {
            string[] array = new string[] { "One", "Two" };

            Assert.That(() => {
                int index = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            Assert.That(() => {
                int index = 2;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            int i1 = 0;
            ThrowHelper.ThrowIfArrayOutOfBounds(array, i1);

            int i2 = 1;
            ThrowHelper.ThrowIfArrayOutOfBounds(array, i2);
        }

        [Test]
        public void ThrowIfArrayBounds_Long()
        {
            string[] array = new string[] { "One", "Two" };

            Assert.That(() => {
                long index = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            Assert.That(() => {
                long index = 2;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            long i1 = 0;
            ThrowHelper.ThrowIfArrayOutOfBounds(array, i1);

            long i2 = 1;
            ThrowHelper.ThrowIfArrayOutOfBounds(array, i2);
        }

        [Test]
        public void ThrowIfArrayBoundsRange_Null()
        {
            string[] array = null;

            Assert.That(() => {
                int index = 0;
                int length = 10;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("array"));

            Assert.That(() => {
                long index = 0;
                long length = 10;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("array"));
        }

        [Test]
        public void ThrowIfArrayBoundsRange()
        {
            string[] array = new string[] { "One", "Two" };

            Assert.That(() => {
                int index = -1;
                int length = 0;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            Assert.That(() => {
                int index = -1;
                int length = 2;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            Assert.That(() => {
                int index = 2;
                int length = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("length"));

            Assert.That(() => {
                int index = 0;
                int length = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("length"));

            Assert.That(() => {
                int index = 1;
                int length = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("length"));

            Assert.That(() => {
                int index = 0;
                int length = 3;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentException>());

            Assert.That(() => {
                int index = 1;
                int length = 2;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentException>());

            Assert.That(() => {
                int index = 2;
                int length = 1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentException>());

            for (int i = 0; i < 2; i++) {
                for (int l = 0; l < 2 - i; i++) {
                    ThrowHelper.ThrowIfArrayOutOfBounds(array, i, l);
                }
            }
        }

        [Test]
        public void ThrowIfArrayBoundsRange_Long()
        {
            string[] array = new string[] { "One", "Two" };

            Assert.That(() => {
                long index = -1;
                long length = 0;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            Assert.That(() => {
                long index = -1;
                long length = 2;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index"));

            Assert.That(() => {
                long index = 2;
                long length = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("length"));

            Assert.That(() => {
                long index = 0;
                long length = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("length"));

            Assert.That(() => {
                long index = 1;
                long length = -1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("length"));

            Assert.That(() => {
                long index = 0;
                long length = 3;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentException>());

            Assert.That(() => {
                long index = 1;
                long length = 2;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentException>());

            Assert.That(() => {
                long index = 2;
                long length = 1;
                ThrowHelper.ThrowIfArrayOutOfBounds(array, index, length);
            }, Throws.TypeOf<ArgumentException>());

            for (long i = 0; i < 2; i++) {
                for (long l = 0; l < 2 - i; i++) {
                    ThrowHelper.ThrowIfArrayOutOfBounds(array, i, l);
                }
            }
        }
    }
}
