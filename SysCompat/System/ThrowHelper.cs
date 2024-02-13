namespace System
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Backwards compatible classes for throw helpers.
    /// </summary>
    /// <remarks>
    /// If using with .NET Core 6.0 or later without considering .NET Framework source compatibility, then use the
    /// native implementations instead.
    /// </remarks>
    public static class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="argument">The reference type argument to validate as not <see langword="null"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <remarks>
        /// Is used to implement CA1510.
        /// <para>
        /// This method works also with .NET 4.0 Framework, so long as the language version is set to C# 10 (due to
        /// <see cref="CallerArgumentExpressionAttribute"/>).
        /// </para>
        /// <para>
        /// If using with .NET Core 6.0 or later without considering .NET Framework source compatibility, then use
        /// <c>ArgumentNullException.ThrowIfNull</c> instead.
        /// </para>
        /// </remarks>
#if NET6_0_OR_GREATER || NET462_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ThrowIfNull(object argument, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
#if NETFRAMEWORK
            if (argument == null)
                throw new ArgumentNullException(paramName);
#else
            if (argument is null)
                throw new ArgumentNullException(paramName);
#endif
        }
    }
}
