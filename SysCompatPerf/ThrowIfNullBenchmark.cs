namespace RJCP.Core
{
    using System;
    using BenchmarkDotNet.Attributes;

    public class ThrowIfNullBenchmark
    {
        private static readonly string TestString = "Test String";

        private static void ThrowIfNull(string value)
        {
            ThrowHelper.ThrowIfNull(value);
        }

        [Benchmark]
        public void ThrowIfNull() => ThrowIfNull(TestString);

#if NET6_0_OR_GREATER
        private static void ThrowIfNull_System(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
        }

        [Benchmark]
        public void ThrowIfNull_System() => ThrowIfNull_System(TestString);
#endif

        private static void ThrowIfNullOrWhiteSpace(string value)
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(value);
        }

        [Benchmark]
        public void ThrowIfNullOrWhiteSpace() => ThrowIfNullOrWhiteSpace(TestString);

#if NET8_0_OR_GREATER
        private static void ThrowIfNullOrWhiteSpace_System(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
        }

        [Benchmark]
        public void ThrowIfNullOrWhiteSpace_System() => ThrowIfNullOrWhiteSpace_System(TestString);
#endif

        private static void ThrowIfZero(int value)
        {
            ThrowHelper.ThrowIfZero(value);
        }

        [Benchmark]
        public void ThrowIfZero() => ThrowIfZero(10);

#if NET8_0_OR_GREATER
        private static void ThrowIfZero_System(int value)
        {
            ArgumentOutOfRangeException.ThrowIfZero(value);
        }

        [Benchmark]
        public void ThrowIfZero_System() => ThrowIfZero_System(10);
#endif
    }
}
