namespace System.Throw
{
    using NUnit.Framework;

    [TestFixture(typeof(int))]
    [TestFixture(typeof(long))]
    [TestFixture(typeof(nint))]
    [TestFixture(typeof(float))]
    [TestFixture(typeof(double))]
    public class ThrowIfNegativeTest<T>
    {
        private static void ThrowIfNegative(object value)
        {
            if (typeof(T) == typeof(int)) {
                int arg = (int)Convert.ChangeType(value, typeof(int));
                ThrowHelper.ThrowIfNegative(arg);
                return;
            } else if (typeof(T) == typeof(long)) {
                long arg = (long)Convert.ChangeType(value, typeof(long));
                ThrowHelper.ThrowIfNegative(arg);
                return;
            } else if (typeof(T) == typeof(nint)) {
                nint arg = (int)Convert.ChangeType(value, typeof(int));
                ThrowHelper.ThrowIfNegative(arg);
                return;
            } else if (typeof(T) == typeof(float)) {
                float arg = (float)Convert.ChangeType(value, typeof(float));
                ThrowHelper.ThrowIfNegative(arg);
                return;
            } else if (typeof(T) == typeof(double)) {
                double arg = (double)Convert.ChangeType(value, typeof(double));
                ThrowHelper.ThrowIfNegative(arg);
                return;
            }
            throw new InvalidOperationException("Invalid type");
        }

        [Test]
        public void ThrowIfNegative()
        {
            Assert.That(() => {
                ThrowIfNegative(-1);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            ThrowIfNegative(0);
            ThrowIfNegative(1);
        }
    }
}
