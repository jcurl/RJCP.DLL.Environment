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
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
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
            if (argument is null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="argument"/> is an empty string.
        /// </summary>
        /// <param name="argument">The reference type argument to validate as not empty.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">string 'paramName' is empty.</exception>
        /// <remarks>
        /// <para>
        /// This method works also with .NET 4.0 Framework, so long as the language version is set to C# 10 (due to
        /// <see cref="CallerArgumentExpressionAttribute"/>).
        /// </para>
        /// <para>
        /// If using with .NET Core 6.0 or later without considering .NET Framework source compatibility, then use
        /// <c>ArgumentException.ThrowIfNullOrEmpty</c> instead.
        /// </para>
        /// </remarks>
#if NET6_0_OR_GREATER || NET462_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ThrowIfNullOrEmpty(string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (string.IsNullOrEmpty(argument)) {
                ThrowIfNull(argument, paramName);
                throw new ArgumentException($"string '{paramName}' may not be an empty string", paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="argument"/> is an empty string.
        /// </summary>
        /// <param name="message">The message to use in the exception.</param>
        /// <param name="argument">The reference type argument to validate as not empty.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">string 'paramName' is empty.</exception>
        /// <remarks>
        /// <para>
        /// This method works also with .NET 4.0 Framework, so long as the language version is set to C# 10 (due to
        /// <see cref="CallerArgumentExpressionAttribute"/>).
        /// </para>
        /// <para>
        /// If using with .NET Core 6.0 or later without considering .NET Framework source compatibility, then use
        /// <c>ArgumentException.ThrowIfNullOrEmpty</c> instead.
        /// </para>
        /// </remarks>
#if NET6_0_OR_GREATER || NET462_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ThrowIfNullOrEmptyMsg(string message, string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (string.IsNullOrEmpty(argument)) {
                ThrowIfNull(argument, paramName);
                throw new ArgumentException(message, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="argument"/> is an empty string or whitespace.
        /// </summary>
        /// <param name="argument">The reference type argument to validate as not empty or whitespace.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">string 'paramName' is empty or whitespace.</exception>
        /// <remarks>
        /// <para>
        /// This method works also with .NET 4.0 Framework, so long as the language version is set to C# 10 (due to
        /// <see cref="CallerArgumentExpressionAttribute"/>).
        /// </para>
        /// <para>
        /// If using with .NET Core 6.0 or later without considering .NET Framework source compatibility, then use
        /// <c>ArgumentException.ThrowIfNullOrWhiteSpace</c> instead.
        /// </para>
        /// </remarks>
#if NET6_0_OR_GREATER || NET462_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ThrowIfNullOrWhiteSpace(string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument)) {
                ThrowIfNull(argument, paramName);
                throw new ArgumentException($"string '{paramName}' may not be empty or whitespace", paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="argument"/> is an empty string or whitespace.
        /// </summary>
        /// <param name="message">The message to use in the exception.</param>
        /// <param name="argument">The reference type argument to validate as not empty or whitespace.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">string 'paramName' is empty or whitespace.</exception>
        /// <remarks>
        /// <para>
        /// This method works also with .NET 4.0 Framework, so long as the language version is set to C# 10 (due to
        /// <see cref="CallerArgumentExpressionAttribute"/>).
        /// </para>
        /// </remarks>
#if NET6_0_OR_GREATER || NET462_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ThrowIfNullOrWhiteSpaceMsg(string message, string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument)) {
                ThrowIfNull(argument, paramName);
                throw new ArgumentException($"string '{paramName}' may not be empty or whitespace", paramName);
            }
        }
    }
}
