namespace RJCP.Core
{
    using System;
    using BenchmarkDotNet.Attributes;

    public class ThrowIfArray
    {
        private static readonly int[] IntArray = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private static void ThrowIfArrayOutOfBounds(int[] array, int offset, int length)
        {
            ThrowHelper.ThrowIfArrayOutOfBounds(array, offset, length);
        }

        [Benchmark]
        public void ThrowIfArrayOutOfBounds() => ThrowIfArrayOutOfBounds(IntArray, 3, 4);
    }
}
