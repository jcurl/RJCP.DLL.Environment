namespace System
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the <paramref name="list"/> is of length zero.
        /// </summary>
        /// <typeparam name="T">A generic type for the list.</typeparam>
        /// <param name="list">The list to validate against.</param>
        /// <param name="listName">The name of the <paramref name="list"/> parameter.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="list"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="list"/> has a length of zero.</exception>
        public static void ThrowIfListEmpty<T>(IList<T> list,
            [CallerArgumentExpression(nameof(list))] string listName = null)
        {
            ThrowIfNull(list, listName);
            if (list.Count is 0)
                throw new ArgumentException($"{listName}.Count must be non-zero.", listName);
        }

        /// <summary>
        /// Throws an exception if <paramref name="index"/> is out of bounds for <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">A generic type for the list.</typeparam>
        /// <param name="list">The list to validate against.</param>
        /// <param name="index">The index to validate as being in <paramref name="list"/>.</param>
        /// <param name="listName">The name of the <paramref name="list"/> parameter.</param>
        /// <param name="indexName">The name of the <paramref name="index"/> parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="index"/> is less than zero, or exceeds the boundary of <paramref name="list"/>.
        /// </exception>
        public static void ThrowIfListOutOfBounds<T>(IList<T> list, int index,
            [CallerArgumentExpression(nameof(list))] string listName = null,
            [CallerArgumentExpression(nameof(index))] string indexName = null)
        {
            ThrowIfNull(list, listName);
            ThrowIfNotBetween(index, 0, list.Count - 1, indexName);
        }

        /// <summary>
        /// Throws an exception if <paramref name="index"/> is out of bounds for <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">A generic type for the list.</typeparam>
        /// <param name="list">The list to validate against.</param>
        /// <param name="index">The index to validate as being in <paramref name="list"/>.</param>
        /// <param name="listName">The name of the <paramref name="list"/> parameter.</param>
        /// <param name="indexName">The name of the <paramref name="index"/> parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="index"/> is less than zero, or exceeds the boundary of <paramref name="list"/>.
        /// </exception>
        public static void ThrowIfListOutOfBounds<T>(IList<T> list, long index,
            [CallerArgumentExpression(nameof(list))] string listName = null,
            [CallerArgumentExpression(nameof(index))] string indexName = null)
        {
            ThrowIfNull(list, listName);
            ThrowIfNotBetween(index, 0, list.Count - 1, indexName);
        }

        /// <summary>
        /// Throws an exception if <paramref name="offset"/> or <paramref name="length"/> can cause an out of bounds
        /// access to <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">A generic type for the list.</typeparam>
        /// <param name="list">The list to validate against.</param>
        /// <param name="offset">The index to validate as being in <paramref name="list"/>.</param>
        /// <param name="length">
        /// The length to validate as being in bounds with <paramref name="offset"/> within <paramref name="list"/>.
        /// </param>
        /// <param name="listName">Name of the <paramref name="list"/> parameter.</param>
        /// <param name="offsetName">Name of the <paramref name="offset"/> parameter.</param>
        /// <param name="lengthName">Name of the <paramref name="length"/> parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is must be zero or positive;
        /// <para>- or -</para>
        /// <paramref name="length"/> is must be zero or positive.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="list"/> out of bounds access with <paramref name="offset"/> and <paramref name="length"/>
        /// exceeding the list length.
        /// </exception>
        public static void ThrowIfListOutOfBounds<T>(IList<T> list, int offset, int length,
            [CallerArgumentExpression(nameof(list))] string listName = null,
            [CallerArgumentExpression(nameof(offset))] string offsetName = null,
            [CallerArgumentExpression(nameof(length))] string lengthName = null)
        {
            ThrowIfNull(list, listName);
            ThrowIfNegative(offset, offsetName);
            ThrowIfNegative(length, lengthName);
            if (offset > list.Count - length)
                throw new ArgumentException($"{listName} out of bounds access with offset '{offset}' and length '{length}' exceeding '{list.Count}'.");
        }

        /// <summary>
        /// Throws an exception if <paramref name="offset"/> or <paramref name="length"/> can cause an out of bounds
        /// access to <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">A generic type for the list.</typeparam>
        /// <param name="list">The list to validate against.</param>
        /// <param name="offset">The index to validate as being in <paramref name="list"/>.</param>
        /// <param name="length">
        /// The length to validate as being in bounds with <paramref name="offset"/> within <paramref name="list"/>.
        /// </param>
        /// <param name="listName">Name of the <paramref name="list"/> parameter.</param>
        /// <param name="offsetName">Name of the <paramref name="offset"/> parameter.</param>
        /// <param name="lengthName">Name of the <paramref name="length"/> parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is must be zero or positive;
        /// <para>- or -</para>
        /// <paramref name="length"/> is must be zero or positive.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="list"/> out of bounds access with <paramref name="offset"/> and <paramref name="length"/>
        /// exceeding the list length.
        /// </exception>
        public static void ThrowIfListOutOfBounds<T>(IList<T> list, long offset, long length,
            [CallerArgumentExpression(nameof(list))] string listName = null,
            [CallerArgumentExpression(nameof(offset))] string offsetName = null,
            [CallerArgumentExpression(nameof(length))] string lengthName = null)
        {
            ThrowIfNull(list, listName);
            ThrowIfNegative(offset, offsetName);
            ThrowIfNegative(length, lengthName);
            if (offset > list.Count - length)
                throw new ArgumentException($"{listName} out of bounds access with offset '{offset}' and length '{length}' exceeding '{list.Count}'.");
        }
    }
}
