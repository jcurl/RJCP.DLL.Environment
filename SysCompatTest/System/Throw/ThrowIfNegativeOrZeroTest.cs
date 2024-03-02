namespace System.Throw
{
    using NUnit.Framework;

    [TestFixture(typeof(int))]
    [TestFixture(typeof(long))]
    [TestFixture(typeof(nint))]
    [TestFixture(typeof(float))]
    [TestFixture(typeof(double))]
    public class ThrowIfNegativeOrZeroTest<T>
    {
        private static void ThrowIfNegativeOrZero(object value)
        {
            if (typeof(T) == typeof(int)) {
                int arg = (int)Convert.ChangeType(value, typeof(int));
                ThrowHelper.ThrowIfNegativeOrZero(arg);
                return;
            } else if (typeof(T) == typeof(long)) {
                long arg = (long)Convert.ChangeType(value, typeof(long));
                ThrowHelper.ThrowIfNegativeOrZero(arg);
                return;
            } else if (typeof(T) == typeof(nint)) {
                nint arg = (int)Convert.ChangeType(value, typeof(int));
                ThrowHelper.ThrowIfNegativeOrZero(arg);
                return;
            } else if (typeof(T) == typeof(float)) {
                float arg = (float)Convert.ChangeType(value, typeof(float));
                ThrowHelper.ThrowIfNegativeOrZero(arg);
                return;
            } else if (typeof(T) == typeof(double)) {
                double arg = (double)Convert.ChangeType(value, typeof(double));
                ThrowHelper.ThrowIfNegativeOrZero(arg);
                return;
            }
            throw new InvalidOperationException("Invalid type");
        }

        [Test]
        public void ThrowIfNegativeOrZero()
        {
            Assert.That(() => {
                ThrowIfNegativeOrZero(-1);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            Assert.That(() => {
                ThrowIfNegativeOrZero(0);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            ThrowIfNegativeOrZero(1);
        }
    }
}
