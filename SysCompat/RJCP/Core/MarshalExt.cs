namespace RJCP.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Extensions for the <see cref="Marshal"/> class.
    /// </summary>
    /// <remarks>
    /// This class provides additional methods and extensions that can be used between .NET 4.0 and newer up to .NET
    /// Core.
    /// <para>
    /// Only some methods are provided. That means that you should change the keyword from <c>Marshal</c> to
    /// <c>MarshalExt</c> in your code when updating. We rely on aggressive inline to ensure that there is no
    /// performance penalty for using these methods.
    /// </para>
    /// </remarks>
    public static class MarshalExt
    {
        /// <summary>
        /// Returns the size of an unmanaged type in bytes.
        /// </summary>
        /// <typeparam name="T">The type whose size is to be returned.</typeparam>
        /// <returns>The size, in bytes, of the type that is specified by the T generic type parameter.</returns>
        /// <remarks>
        /// You can use this method when you do not have a structure. The layout must be sequential or explicit. The
        /// size returned is the size of the unmanaged type.The unmanaged and managed sizes of an object can differ.For
        /// character types, the size is affected by the CharSet value applied to that class.
        /// <para>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// </para>
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///var size = Marshal.SizeOf(typeof(MyClass));
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///var size = Marshal.SizeOf<MyClass>();
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///var size = MarshalExt.SizeOf<MyClass>();
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int SizeOf<T>()
        {
#if NET462_OR_GREATER || NETCOREAPP
            return Marshal.SizeOf<T>();
#else
            return Marshal.SizeOf(typeof(T));
#endif
        }

        // We don't provide an implementation for:
        //
        //  static int SizeOf<T>(T structure)
        //
        // because the compiler will use the `object` signature instead, thus using the most efficient version
        // available for the platform being targetted.

        /// <summary>
        /// Marshals data from an unmanaged block of memory to a newly allocated managed object of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to which the data is to be copied. This must be a formatted class or a structure.
        /// </typeparam>
        /// <param name="ptr">
        /// The type of the object to which the data is to be copied. This must be a formatted class or a structure.
        /// </param>
        /// <returns>A managed object that contains the data that the ptr parameter points to.</returns>
        /// <remarks>
        /// <para>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// </para>
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///MyClass c = (MyClass)Marshal.PtrToStructure(ipVar, typeof(MyClass));
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///MyClass c = Marshal.PtrToStructure<MyClass>(ipVar);
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///MyClass c = MarshalExt.PtrToStructure<MyClass>(ipVar);
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T PtrToStructure<T>(IntPtr ptr)
        {
#if NET462_OR_GREATER || NETCOREAPP
            return Marshal.PtrToStructure<T>(ptr);
#else
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
#endif
        }

        // We don't provide an implementation for:
        //
        //  static void PtrToStructure<T>(IntPtr ptr, T structure)
        //
        // because the compiler will use the `object` signature instead, thus using the most efficient version
        // available for the platform being targetted.

        /// <summary>
        /// Frees all substructures of a specified type that the specified unmanaged memory block points to.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the formatted structure. This provides the layout information necessary to delete the buffer in
        /// the ptr parameter.
        /// </typeparam>
        /// <param name="ptr">
        /// A pointer to an unmanaged block of memory.
        /// </param>
        /// <remarks>
        /// You can use this method to free reference type fields, such as strings, of an unmanaged structure. Unlike
        /// its fields, a structure can be a value type or a reference type. Value type structures that contain value
        /// type fields (all blittable) have no references whose memory must be freed. The Marshal.StructureToPtr
        /// method uses this method to prevent memory leaks when reusing memory occupied by a structure.
        /// <para>
        /// DestroyStructure calls the COM SysFreeString function, which, in turn, frees an allocated string.
        /// </para>
        /// <para>
        /// This method is needed for targetting both .NET Framework and .NET Core projects together. It is not needed
        /// for .NET Core only projects, that already natively support checking if the methods is defined using
        /// generics. Projects that support .NET Framework in addition do not have this and using the non-generic
        /// versions result in the IDE warning CA2263.
        /// </para>
        /// <para>Normally, code in .NET Framework would look like:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///Marshal.DestroyStructure(ipVar, typeof(MyClass));
        ///]]>
        /// </code>
        /// <para>
        /// This results in the warning CA2263 on .NET Core, and is resolved with the following code (that uses
        /// generics) which does not compile on .NET Framework:
        /// </para>
        /// <code language="csharp">
        ///<![CDATA[
        ///Marshal.DestroyStructure<MyClass>(ipVar);
        ///]]>
        /// </code>
        /// <para>The solution is to use this method in the form:</para>
        /// <code language="csharp">
        ///<![CDATA[
        ///MarshalExt.DestroyStructure<MyClass>(ipVar);
        ///]]>
        /// </code>
        /// </remarks>
#if NET45_OR_GREATER || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void DestroyStructure<T>(IntPtr ptr)
        {
#if NET462_OR_GREATER || NETCOREAPP
            Marshal.DestroyStructure<T>(ptr);
#else
            Marshal.DestroyStructure(ptr, typeof(T));
#endif
        }
    }
}
