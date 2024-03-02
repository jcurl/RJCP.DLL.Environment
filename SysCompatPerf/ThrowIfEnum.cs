namespace RJCP.Core
{
    using System;
    using BenchmarkDotNet.Attributes;

    public class ThrowIfEnum
    {
        private enum MyEnum { Zero, One, Two, Three };

        private static void ThrowIfEnumUndefined(MyEnum value)
        {
            ThrowHelper.ThrowIfEnumUndefined(value);
        }

        [Benchmark]
        public void ThrowIfEnumUndefined() => ThrowIfEnumUndefined(MyEnum.Zero);

        [Flags]
        private enum FileFlags
        {
            None = 0,
            Read = 1,
            Write = 2,
            Execute = 4
        }

        private static void ThrowIfReadFlagMissing(FileFlags value)
        {
            ThrowHelper.ThrowIfEnumHasNoFlag(value, FileFlags.Read);
        }

        [Benchmark]
        public void ThrowIfEnumHasFlag() => ThrowIfReadFlagMissing((FileFlags)7);
    }
}
