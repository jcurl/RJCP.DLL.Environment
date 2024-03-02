namespace System.Throw
{
    using NUnit.Framework;

    [TestFixture(typeof(int))]
    [TestFixture(typeof(long))]
    [TestFixture(typeof(nint))]
    [TestFixture(typeof(float))]
    [TestFixture(typeof(double))]
    [TestFixture(typeof(uint))]
    [TestFixture(typeof(ulong))]
    [TestFixture(typeof(nuint))]
    public class ThrowIfZeroTest<T>
    {
        private static void ThrowIfZero(object value)
        {
            if (typeof(T) == typeof(int)) {
                int arg = (int)Convert.ChangeType(value, typeof(int));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(long)) {
                long arg = (long)Convert.ChangeType(value, typeof(long));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(nint)) {
                nint arg = (int)Convert.ChangeType(value, typeof(int));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(float)) {
                float arg = (float)Convert.ChangeType(value, typeof(float));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(double)) {
                double arg = (double)Convert.ChangeType(value, typeof(double));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(uint)) {
                uint arg = (uint)Convert.ChangeType(value, typeof(uint));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(ulong)) {
                ulong arg = (ulong)Convert.ChangeType(value, typeof(ulong));
                ThrowHelper.ThrowIfZero(arg);
                return;
            } else if (typeof(T) == typeof(nuint)) {
                nuint arg = (uint)Convert.ChangeType(value, typeof(uint));
                ThrowHelper.ThrowIfZero(arg);
                return;
            }
            throw new InvalidOperationException("Invalid type");
        }

        [Test]
        public void ThrowIfZero()
        {
            Assert.That(() => {
                ThrowIfZero(0);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            ThrowIfZero(1);
        }
    }
}
