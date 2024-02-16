# RJCP.Core.SysCompat <!-- omit in toc -->

This library provides implementation details that allow some backward
compatibility from newer .NET Core features to older .NET Framework libraries
when using newer C# Language versions.

- [1. .NET Framework Usage](#1-net-framework-usage)
  - [1.1. Conditional Version Upgrade](#11-conditional-version-upgrade)
  - [1.2. NuGet Package Reference](#12-nuget-package-reference)
- [2. Extensions](#2-extensions)
  - [2.1. CallerArgumentExpression](#21-callerargumentexpression)
  - [2.2. Throw Helpers](#22-throw-helpers)
    - [2.2.1. How Throw Helpers get the Argument Name](#221-how-throw-helpers-get-the-argument-name)
  - [2.3. SupportedOSPlatform](#23-supportedosplatform)
- [3. C# Language Features](#3-c-language-features)
  - [3.1. Init Properties](#31-init-properties)
    - [3.1.1. Feature Description](#311-feature-description)
    - [3.1.2. Enablement](#312-enablement)
  - [3.2. Required Properties](#32-required-properties)
    - [3.2.1. Enablement](#321-enablement)

## 1. .NET Framework Usage

To use this library with newer C# compiler versions, extend this in your .NET
SDK project file:

### 1.1. Conditional Version Upgrade

By
[default](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version)
the highest version of .NET Framework is C# 7.3. Newer compilers can still
target older frameworks.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net40;net60</TargetFrameworks>
    ...

    <LangVersion Condition="'$(TargetFrameworkIdentifier)' == '.NETFramework'">10</LangVersion>
  </PropertyGroup>
</Project>
```

### 1.2. NuGet Package Reference

Add to your project a reference to `RJCP.Core.SysCompat` to your project for
.NET Core and .NET Framework.

## 2. Extensions

### 2.1. CallerArgumentExpression

| <!-- -->    | <!-- -->       |
| ----------- | -------------- |
| LangVersion | 10             |
| Framework   | .NET Framework |

C# 10 and later support the attribute `CallerArgumentExpressionAttribute` that
is provided in .NET 6.0 and later. On .NET Framework, this package provides the
attribute, that the compiler can inject code. Ensure the `LangVersion` is 10 or
later.

It is now possible to provide helper methods that can use the name of an
argument and pass it on as further arguments.

For details on how this is used, please study the implementation of
`ThrowHelper.ThrowIfNull(obj)`.

### 2.2. Throw Helpers

| <!-- -->    | <!-- -->                |
| ----------- | ----------------------- |
| LangVersion | 7.3                     |
| Framework   | .NET Framework and Core |

In .NET 6.0 and later, there are the helpers

```csharp
ArgumentNullException.ThrowIfNull(param);
ArgumentException.ThrowIfNullOrEmpty(param);
ArgumentException.ThrowIfNullOrWhiteSpace(param);
```

Usage within your .NET Framework and .NET Core projects can use instead:

```csharp
using System;

ThrowHelper.ThrowIfNull(obj);
ThrowHelper.ThrowIfNullOrEmpty(strparam);
ThrowHelper.ThrowIfNullOrWhiteSpace(strparam);
```

The Package is compiled with the language version C# 10, so that projects using
this library for this feature do not need to upgrade.

The additional methods are provided, which are not part of the framework. These
methods are useful if you have your own translated strings that you wish to
provide the user. Note that the method name must change, due to the overload
`ThrowIfNullOrEmpty(string, string)` exists.

```csharp
using System;

ThrowHelper.ThrowIfNullOrEmptyMsg(message, strparam);
ThrowHelper.ThrowIfNullOrWhiteSpace(message, strparam);
```

#### 2.2.1. How Throw Helpers get the Argument Name

C# 10 and later support the attribute `CallerArgumentExpressionAttribute` that
is provided in .NET 6.0 and later. On .NET Framework, it uses the attribute
provided by this package, and the Roslyn compiler will still inject the
necessary code to provide the attribute name.

```csharp
public static void ThrowIfNull(object argument,
                               [CallerArgumentExpression(nameof(argument))] string paramName = null)
```

### 2.3. SupportedOSPlatform

| <!-- -->    | <!-- -->       |
| ----------- | -------------- |
| LangVersion | 7.3            |
| Framework   | .NET Framework |

With .NET 6.0 and later, code analysis warning [CA1416 Call Site
Reachable](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1416)
can significantly support identifying potential failures in multi-target code.

To avoid unnecessary conditional compiles, it is convenient to define the
[`SupportedOSPlatform`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.versioning.supportedosplatformattribute?view=net-8.0)
attribute, that the same code can compile for .NET Framework.

## 3. C# Language Features

### 3.1. Init Properties

| <!-- -->    | <!-- -->                |
| ----------- | ----------------------- |
| LangVersion | 9                       |
| Framework   | .NET Framework and Core |

It is not recommended enabling this for .NET Framework for reusable NuGet packages.

#### 3.1.1. Feature Description

C# 9 introduces two new keywords
[init](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init).
Properties are initialised optionally. The benefit is to define properties that
are initialised at the same time the constructor is called, such that the
properties are set without having to set them within the constructor.

For example, instead of:

```csharp
public class MyClass {
  public MyClass(string prop) {
    Prop = prop;
  }

  public string Prop { get; }
}

MyClass c = new("value");
```

we can instead have:

```csharp
public class MyClass {
  public string Prop { get; init; }
}

MyClass c = new() {
  Prop = "value"
};
```

Benefit is more obvious with more properties (e.g. 3 or more), that values are
defined without having to have a lot of arguments in constructors, or variations
of parameters in the constructor.

#### 3.1.2. Enablement

The `RJCP.Core.SysCompat` library unfortunately cannot help, which is why the
feature is described here. In your assembly for .NET Framework, ensure that:

- The language version is set to 9 or greater (.NET Core 5 or later) in both the
  library defining the `init` property, and in the executable consuming the
  class with the `init` property.

  ```xml
  <PropertyGroup>
    <TargetFrameworks>net40;net462;net60</TargetFrameworks>
    ...
    <LangVersion Condition="$(TargetFramework.StartsWith('net4'))">10</LangVersion>
  ```

- Add the following class, compiled only for .NET Framework (or .NET 2.1 or
  earlier also). Putting this into the `RJCP.Core.SysCompat.dll` library does
  not work. This should be in the library defining the `init` property (not
  needed in the executable consuming the class with the `init` property).

  ```csharp
  namespace System.Runtime.CompilerServices
  {
      using System.ComponentModel;

  #if NETFRAMEWORK || !NET5_0_OR_GREATER
      [EditorBrowsable(EditorBrowsableState.Never)]
      internal static class IsExternalInit { }
  #endif
  }
  ```

### 3.2. Required Properties

| <!-- -->    | <!-- -->                |
| ----------- | ----------------------- |
| LangVersion | 11                      |
| Framework   | .NET Framework and Core |

Along with
[init](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init),
C# 11 adds the keyword
[required](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required),
which enforces that initialised properties must be provided.

#### 3.2.1. Enablement

If compiling with C# 10 or earlier:

| Severity | Code   | Description                                                                                               |
| -------- | ------ | --------------------------------------------------------------------------------------------------------- |
| Error    | CS8936 | Feature 'required members' is not available in C# 10.0. Please use language version 11.0 or greater.      |
| Error    | CS0656 | Missing compiler required member 'System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute..ctor' |
| Error    | CS0656 | Missing compiler required member 'System.Runtime.CompilerServices.RequiredMemberAttribute..ctor'          |

- Set the language version to 11 or greater (.NET Core 7 or later) in both the
  library defining the `required` property, and in the executable consuming the
  class with the `required` property.

  ```xml
  <PropertyGroup>
    <TargetFrameworks>net40;net462;net60</TargetFrameworks>
    ...
    <LangVersion>11</LangVersion>
  ```

- Add the following class (not needed in the executable consuming the class with
  the `required` property)

  ```csharp
  namespace System.Runtime.CompilerServices
  {
      using System.ComponentModel;

  #if NETFRAMEWORK || !NET5_0_OR_GREATER
      [EditorBrowsable(EditorBrowsableState.Never)]
      internal static class IsExternalInit { }
  #endif

  #if NETFRAMEWORK || !NET7_0_OR_GREATER
      [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field |
          AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
      internal sealed class RequiredMemberAttribute : Attribute { }

      [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
      internal sealed class CompilerFeatureRequiredAttribute : Attribute
      {
          public CompilerFeatureRequiredAttribute(string featureName)
          {
              FeatureName = featureName;
          }

          public string FeatureName { get; }
          public bool IsOptional { get; init; }

          public const string RefStructs = nameof(RefStructs);
          public const string RequiredMembers = nameof(RequiredMembers);
      }
  #endif
  }
  ```
