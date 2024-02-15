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
    - [2.2.1. Discussion of How it Works](#221-discussion-of-how-it-works)
  - [2.3. SupportedOSPlatform](#23-supportedosplatform)

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

#### 2.2.1. Discussion of How it Works

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
