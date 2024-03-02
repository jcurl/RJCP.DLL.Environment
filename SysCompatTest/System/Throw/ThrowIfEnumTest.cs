namespace System.Throw
{
    using NUnit.Framework;

    [TestFixture]
    public class ThrowIfEnumTest
    {
        private enum MyEnum { Zero, One, Two };

        [Test]
        public void ThrowIfEnumUndefined()
        {
            MyEnum en1 = MyEnum.Zero;
            ThrowHelper.ThrowIfEnumUndefined(en1);

            MyEnum en2 = 0;
            ThrowHelper.ThrowIfEnumUndefined(en2);

            Assert.That(() => {
                MyEnum en3 = (MyEnum)4;
                ThrowHelper.ThrowIfEnumUndefined(en3);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("en3"));
        }

        [Flags]
        private enum FileFlags
        {
            None = 0,
            Read = 1,
            Write = 2,
            Execute = 4
        }

        [Test]
        public void ThrowIfEnumUndefinedFlags()
        {
            FileFlags f1 = 0;
            ThrowHelper.ThrowIfEnumUndefined(f1);

            // Note, even if all flags are defined, the exception is still raised, as there is no value "6" in the
            // enumeration.
            Assert.That(() => {
                FileFlags f2 = (FileFlags)6;
                ThrowHelper.ThrowIfEnumUndefined(f2);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("f2"));

            Assert.That(() => {
                FileFlags f3 = (FileFlags)15;
                ThrowHelper.ThrowIfEnumUndefined(f3);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("f3"));
        }

        [Test]
        public void ThrowIfEnumNoFlags()
        {
            Assert.That(() => {
                FileFlags f1 = 0;
                ThrowHelper.ThrowIfEnumHasNoFlag(f1, FileFlags.Read);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("f1"));

            FileFlags f2 = (FileFlags)6;
            Assert.That(() => {
                ThrowHelper.ThrowIfEnumHasNoFlag(f2, FileFlags.Read);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("f2"));

            ThrowHelper.ThrowIfEnumHasNoFlag(f2, FileFlags.Write);
            ThrowHelper.ThrowIfEnumHasNoFlag(f2, FileFlags.Execute);
            ThrowHelper.ThrowIfEnumHasNoFlag(f2, FileFlags.Write | FileFlags.Execute);

            Assert.That(() => {
                ThrowHelper.ThrowIfEnumHasNoFlag(f2, FileFlags.Read | FileFlags.Write);
            }, Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("f2"));

            FileFlags f4 = (FileFlags)15;
            ThrowHelper.ThrowIfEnumHasNoFlag(f4, FileFlags.Read);
        }
    }
}
