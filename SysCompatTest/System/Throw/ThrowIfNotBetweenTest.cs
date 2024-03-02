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
    [TestFixture(typeof(EventLine))]
    public class ThrowIfNotBetweenTest<T>
    {
        private static void ThrowIfNotBetween(object value, object lower, object upper)
        {
            if (value is not null && lower is not null && upper is not null) {
                if (typeof(T) == typeof(int)) {
                    int arg = (int)Convert.ChangeType(value, typeof(int));
                    int argLower = (int)Convert.ChangeType(lower, typeof(int));
                    int argUpper = (int)Convert.ChangeType(upper, typeof(int));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(long)) {
                    long arg = (long)Convert.ChangeType(value, typeof(long));
                    long argLower = (long)Convert.ChangeType(lower, typeof(long));
                    long argUpper = (long)Convert.ChangeType(upper, typeof(long));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(nint)) {
                    nint arg = (int)Convert.ChangeType(value, typeof(int));
                    nint argLower = (int)Convert.ChangeType(lower, typeof(int));
                    nint argUpper = (int)Convert.ChangeType(upper, typeof(int));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(float)) {
                    float arg = (float)Convert.ChangeType(value, typeof(float));
                    float argLower = (float)Convert.ChangeType(lower, typeof(float));
                    float argUpper = (float)Convert.ChangeType(upper, typeof(float));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(double)) {
                    double arg = (double)Convert.ChangeType(value, typeof(double));
                    double argLower = (double)Convert.ChangeType(lower, typeof(double));
                    double argUpper = (double)Convert.ChangeType(upper, typeof(double));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(uint)) {
                    uint arg = (uint)Convert.ChangeType(value, typeof(uint));
                    uint argLower = (uint)Convert.ChangeType(lower, typeof(uint));
                    uint argUpper = (uint)Convert.ChangeType(upper, typeof(uint));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(ulong)) {
                    ulong arg = (ulong)Convert.ChangeType(value, typeof(ulong));
                    ulong argLower = (ulong)Convert.ChangeType(lower, typeof(ulong));
                    ulong argUpper = (ulong)Convert.ChangeType(upper, typeof(ulong));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                } else if (typeof(T) == typeof(nuint)) {
                    nuint arg = (uint)Convert.ChangeType(value, typeof(uint));
                    nuint argLower = (uint)Convert.ChangeType(lower, typeof(uint));
                    nuint argUpper = (uint)Convert.ChangeType(upper, typeof(uint));
                    ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                    return;
                }
            }
            if (typeof(T) == typeof(EventLine)) {
                EventLine arg = null;
                EventLine argLower = null;
                EventLine argUpper = null;
                if (value is not null) {
                    int sev = (int)Convert.ChangeType(value, typeof(int));
                    arg = new(sev, "Message 1");
                }
                if (lower is not null) {
                    int sev = (int)Convert.ChangeType(lower, typeof(int));
                    argLower = new(sev, "Message 2");
                }
                if (upper is not null) {
                    int sev = (int)Convert.ChangeType(upper, typeof(int));
                    argUpper = new(sev, "Message 2");
                }
                ThrowHelper.ThrowIfNotBetween(arg, argLower, argUpper);
                return;
            }
            throw new InvalidOperationException("Invalid type");
        }

        [Test]
        public void ThrowIfGreaterThan()
        {
            Assert.That(() => {
                ThrowIfNotBetween(0, 1, 10);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            ThrowIfNotBetween(1, 1, 10);
            ThrowIfNotBetween(5, 1, 10);
            ThrowIfNotBetween(10, 1, 10);

            Assert.That(() => {
                ThrowIfNotBetween(11, 1, 10);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            if (typeof(T) == typeof(EventLine)) {
                EventLine arg1 = new(10, "Foo");
                EventLine arg2 = new(50, "Bar");
                EventLine argn = null;

                ThrowHelper.ThrowIfNotBetween(argn, null, arg2);
                ThrowHelper.ThrowIfNotBetween(arg1, null, arg2);

                Assert.That(() => {
                    ThrowHelper.ThrowIfNotBetween(argn, arg1, arg2);
                }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("argn"));
            }
        }
    }
}
