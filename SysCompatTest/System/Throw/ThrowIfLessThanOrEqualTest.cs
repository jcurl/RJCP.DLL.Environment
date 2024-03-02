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
    public class ThrowIfLessThanOrEqualTest<T>
    {
        private static void ThrowIfLessThanOrEqual(object value, object other)
        {
            if (value is not null && other is not null) {
                if (typeof(T) == typeof(int)) {
                    int arg = (int)Convert.ChangeType(value, typeof(int));
                    int argOther = (int)Convert.ChangeType(other, typeof(int));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(long)) {
                    long arg = (long)Convert.ChangeType(value, typeof(long));
                    long argOther = (long)Convert.ChangeType(other, typeof(long));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(nint)) {
                    nint arg = (int)Convert.ChangeType(value, typeof(int));
                    nint argOther = (int)Convert.ChangeType(other, typeof(int));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(float)) {
                    float arg = (float)Convert.ChangeType(value, typeof(float));
                    float argOther = (float)Convert.ChangeType(other, typeof(float));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(double)) {
                    double arg = (double)Convert.ChangeType(value, typeof(double));
                    double argOther = (double)Convert.ChangeType(other, typeof(double));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(uint)) {
                    uint arg = (uint)Convert.ChangeType(value, typeof(uint));
                    uint argOther = (uint)Convert.ChangeType(other, typeof(uint));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(ulong)) {
                    ulong arg = (ulong)Convert.ChangeType(value, typeof(ulong));
                    ulong argOther = (ulong)Convert.ChangeType(other, typeof(ulong));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                } else if (typeof(T) == typeof(nuint)) {
                    nuint arg = (uint)Convert.ChangeType(value, typeof(uint));
                    nuint argOther = (uint)Convert.ChangeType(other, typeof(uint));
                    ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                    return;
                }
            }
            if (typeof(T) == typeof(EventLine)) {
                EventLine arg = null;
                EventLine argOther = null;
                if (value is not null) {
                    int sev = (int)Convert.ChangeType(value, typeof(int));
                    arg = new(sev, "Message 1");
                }
                if (other is not null) {
                    int sev = (int)Convert.ChangeType(other, typeof(int));
                    argOther = new(sev, "Message 2");
                }
                ThrowHelper.ThrowIfLessThanOrEqual(arg, argOther);
                return;
            }
            throw new InvalidOperationException("Invalid type");
        }

        [Test]
        public void ThrowIfLessThanOrEqual()
        {
            Assert.That(() => {
                ThrowIfLessThanOrEqual(0, 0);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            Assert.That(() => {
                ThrowIfLessThanOrEqual(10, 10);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            ThrowIfLessThanOrEqual(10, 0);

            Assert.That(() => {
                ThrowIfLessThanOrEqual(0, 10);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("arg"));

            if (typeof(T) == typeof(EventLine)) {
                EventLine arg1 = new(50, "Foo");
                EventLine argn = null;

                Assert.That(() => {
                    ThrowHelper.ThrowIfLessThanOrEqual(argn, null);
                }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("argn"));

                Assert.That(() => {
                    ThrowHelper.ThrowIfLessThanOrEqual(argn, arg1);
                }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("argn"));
            }
        }
    }
}
