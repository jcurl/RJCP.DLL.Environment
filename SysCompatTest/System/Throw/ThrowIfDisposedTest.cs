namespace System.Throw
{
    using NUnit.Framework;

    [TestFixture]
    public class ThrowIfDisposedTest
    {
        [Test]
        public void ThrowIfDisposedObject()
        {
            Assert.That(() => {
                ThrowHelper.ThrowIfDisposed(true, this);
            }, Throws.TypeOf<ObjectDisposedException>()
                .With.Property("ObjectName").EqualTo("System.Throw.ThrowIfDisposedTest"));

            ThrowHelper.ThrowIfDisposed(false, this);
        }

        [Test]
        public void ThrowIfDisposedType()
        {
            Assert.That(() => {
                ThrowHelper.ThrowIfDisposed(true, typeof(ThrowIfDisposedTest));
            }, Throws.TypeOf<ObjectDisposedException>()
                .With.Property("ObjectName").EqualTo("System.Throw.ThrowIfDisposedTest"));

            ThrowHelper.ThrowIfDisposed(false, typeof(ThrowIfDisposedTest));
        }
    }
}
