namespace RJCP.Core
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extensions for the <see cref="Enum"/> object.
    /// </summary>
    /// <remarks>
    /// This class provides additional methods and extensions that can be used between .NET 4.0 and newer up to .NET Core.
    /// </remarks>
    public static class EnumExt
    {
        /// <summary>
        /// Determines whether the specified value is defined in the <see cref="Enum"/> type.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
        /// <param name="value">The value to test if it is within the <see cref="Enum"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the specified value is defined; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///if (!Enum.IsDefined(typeof(ConsoleColor), Console.ForegroundColor)) return false;
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///if (!Enum.IsDefined(Console.ForegroundColor)) return false;
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///if (!Console.ForegroundColor.IsDefined()) return false;
        ///]]>
        /// </code>
        /// <para>
        /// An alternative way to drop in replacement for these methods is to use type aliasing in .NET Framework
        /// projects:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///#if NETFRAMEWORK
        ///using Enum = RJCP.Core.EnumExt;
        ///#endif
        ///
        ///if (!Enum.IsDefined(Console.ForegroundColor)) return false;
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsDefined<TEnum>(this TEnum value) where TEnum : struct, System.Enum
        {
#if NETFRAMEWORK
            return System.Enum.IsDefined(typeof(TEnum), value);
#else
            return System.Enum.IsDefined(value);
#endif
        }

        /// <summary>
        /// Gets the values that are defined by the enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
        /// <returns>An array of values in the <see cref="Enum"/>.</returns>
        /// <remarks>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///foreach (var x in Enum.GetValues(typeof(ConsoleColor))) { ... }
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///foreach (var x in Enum.GetValues<ConsoleColor>()) { ... }
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///foreach (var x in EnumExt.GetValues<ConsoleColor>()) { ... }
        ///]]>
        /// </code>
        /// <para>
        /// An alternative way to drop in replacement for these methods is to use type aliasing in .NET Framework
        /// projects:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///#if NETFRAMEWORK
        ///using Enum = RJCP.Core.EnumExt;
        ///#endif
        ///
        ///var valuesExt = Enum.GetValues<ConsoleColor>();
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TEnum[] GetValues<TEnum>() where TEnum : struct, System.Enum
        {
#if NETFRAMEWORK
            return System.Enum.GetValues(typeof(TEnum)).OfType<TEnum>().ToArray();
#else
            return System.Enum.GetValues<TEnum>();
#endif
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// specified by TEnum to an equivalent enumerated object. A parameter specifies whether the operation is
        /// case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">
        /// <see langword="true"/> to ignore case; <see langword="false"/> to regard case.
        /// </param>
        /// <returns>
        /// An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="TEnum"/> is not an Enum type.
        /// <para>- or -</para>
        /// <paramref name="value"/> does not contain enumeration information.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), "Red", true);
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///ConsoleColor color = Enum.Parse<ConsoleColor>("Red", true);
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///ConsoleColor color = EnumExt.Parse<ConsoleColor>("Red", true);
        ///]]>
        /// </code>
        /// <para>
        /// An alternative way to drop in replacement for these methods is to use type aliasing in .NET Framework
        /// projects:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///#if NETFRAMEWORK
        ///using Enum = RJCP.Core.EnumExt;
        ///#endif
        ///
        ///ConsoleColor color = Enum.Parse<ConsoleColor>("Red", true);
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
#if NETFRAMEWORK
            return (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
#else
            return System.Enum.Parse<TEnum>(value, ignoreCase);
#endif
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// specified by TEnum to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>
        /// An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="TEnum"/> is not an Enum type.
        /// <para>- or -</para>
        /// <paramref name="value"/> does not contain enumeration information.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), "Red");
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///ConsoleColor color = Enum.Parse<ConsoleColor>("Red");
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///ConsoleColor color = EnumExt.Parse<ConsoleColor>("Red");
        ///]]>
        /// </code>
        /// <para>
        /// An alternative way to drop in replacement for these methods is to use type aliasing in .NET Framework
        /// projects:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///#if NETFRAMEWORK
        ///using Enum = RJCP.Core.EnumExt;
        ///#endif
        ///
        ///ConsoleColor color = Enum.Parse<ConsoleColor>("Red");
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
#if NETFRAMEWORK
            return (TEnum)System.Enum.Parse(typeof(TEnum), value);
#else
            return System.Enum.Parse<TEnum>(value);
#endif
        }
    }
}
