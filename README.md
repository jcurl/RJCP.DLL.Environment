# RJCP.Environment <!-- omit in toc -->

This library contains miscellaneous routines that are useful for getting
information about the environment.

This document covers two assemblies, `RJCP.Core.Environment` and
`RJCP.Core.Environment.Version`

- [1. Environment Features](#1-environment-features)
  - [1.1. Platform Version](#11-platform-version)
  - [1.2. XDG Specification on Linux](#12-xdg-specification-on-linux)
- [2. Environment.Version Features](#2-environmentversion-features)
  - [2.1. Windows Version](#21-windows-version)
  - [2.2. .NET Framework Version](#22-net-framework-version)
- [3. Tools](#3-tools)
  - [3.1. WinVersion](#31-winversion)
  - [3.2. NetVersion](#32-netversion)
- [4. Release History](#4-release-history)
  - [4.1. Environment](#41-environment)
    - [4.1.1. Version 0.3.0](#411-version-030)
  - [4.2. Environment.Version](#42-environmentversion)
    - [4.2.1. Version 0.3.2](#421-version-032)
    - [4.2.2. Version 0.3.1](#422-version-031)
    - [4.2.3. Version 0.3.0](#423-version-030)
  - [4.3. Version 0.2.0](#43-version-020)

## 1. Environment Features

### 1.1. Platform Version

Provides a standard way across .NET Framework and .NET Core. .NET 6.0 and later
have introduced their own mechanism, so if your projects don't target older
versions, use the immplementations from .NET Core.

- `IsWinNT()`
- `IsUnix()` for Linux (support for MacOS is removed as this is not tested).

These APIs support the newer `SupportedOSPlatform` attributes in .NET Core 6.0
and later (the attribute is from .NET 5.0, but .NET 6.0 supports the guards).

In addition, further runtimes can be detected:

- `IsMonoClr()`
- `IsMSys()` by checking environment variables - useful with
  `RJCP.Core.CommandLine`.
- `IsCygwin()` by checking environment variables - useful with
  `RJCP.Core.CommandLine`.

### 1.2. XDG Specification on Linux

The `Platform.GetFolderPath` method supports getting standard paths according to
the [XDG
standard](https://specifications.freedesktop.org/basedir-spec/basedir-spec-latest.html).

The following properties are supported:

- `Environment.SpecialFolder.LocalApplicationData`
- `Environment.SpecialFolder.CommonApplicationData`
- Otherwise the base .NET implementation is used. This might change in the
  future if the XDG supports further features.

Additionally, the APIs `Xdg.GetFolderPath` is provided with:

- `Xdg.SpecialFolder.LocalApplicationData`
- `Xdg.SpecialFolder.CommonApplicationData`
- `Xdg.SpecialFolder.CacheData`

that rely on `XDG_` environment variables.

## 2. Environment.Version Features

Since version 0.3.0, functionality has moved from `Environment` to here.

### 2.1. Windows Version

With `WinVersion.LocalMachine` get the version of the current operating system
using `NTDLL`. You can compare against other versions in detail, get Windows
version information, useful for informational reports. Use the existing
functionality in the .NET framework to test against features, and do not use the
Windows version.

For development details see:

- [WinVersionApiQuery](./WinVersionApiQuery/README.md) for a tool that runs
  natively on Windows to capture the output of the Win32 API used for getting
  version information. This is used later as part of testing to emulate the
  APIs, without having to run tests on every version of Windows.
  - [binaries](./WinVersionApiQuery/binary) in the `./WinVersionApiQuery/binary`
    folder. Documentation describes what's compiled and tested.
- [API Versions](./Environment.VersionTest/TestResources/WinVersion/README.md)
  contain captured files, as well as a table showing what APIs are present,
  dependent on the Operating System and the Service Pack.

### 2.2. .NET Framework Version

Use `NetVersions` to enumerate a list of installed versions of .NET Framework,
and the current runtime in use.

To date, there is no documented standard on querying the versions of the .NET
Core runtimes, so this isn't offered.

It checks against MONO on Linux as well as Windows.

## 3. Tools

### 3.1. WinVersion

The repository offers a tool (not provided as a binary) that can be used to show
windows version information.

### 3.2. NetVersion

The repository offers a tool (not provided as a binary) that can be used to show
the .NET Framework version information.

## 4. Release History

### 4.1. Environment

#### 4.1.1. Version 0.3.0

Features:

- Add function to test if binary is running from a Cygwin Shell (DOTNET-931)

Quality:

- Add `README.md` to NuGet package (DOTNET-805)
- Move out Version information, so that there are less dependencies and remove
  cyclic dependencies, as most code uses this library for platform information.
  (DOTNET-851)
- Upgrade to .NET Core 6.0 (DOTNET-936)
- Add references for OS Compatibility (DOTNET-938, DOTNET-942, DOTNET-959)

### 4.2. Environment.Version

#### 4.2.1. Version 0.3.2

API Changes:

- WinVersion: `IsExtendedPropSet` is removed, as it provides no information and

Features:

- WinVersion: Support for Windows 11 24H2 (DOTNET-1039).
- WinVersion: Support for Windows Server 2025 (DOTNET-1041).
- WinVersion: Extend product information for SDK 26100 (DOTNET-1042).
- WinVersion: Provide `WinVersionQueryApi.exe` to dump output of Windows API for
  Windows nt 3.51 up to Windows 11 24H2 (DOTNET-1045, DOTNET-1051).
- WinVersion: Allow interpreting Windows version from an XML file, provided by
  `WinVersionQueryApi`. This allows backward compatibility testing without
  needing to test on the target (DOTNET-1044).
- WinVersion: Tested on WinNT 3.51 (DOTNET-1051).
- WinVersion: Tested on WinNT 4.0 SP1 and SP6. Service Pack is inferred on older
  versions (DOTNET-1051).
- WinVersion: Tested on Windows 2000 SP4. Fixed Service Pack string (CSDVersion)
  on Windows 2000 (DOTNET-1051).

BugFixes:

- WinVersion: For Operating Systems that don't have `IsWow64Process2` and uses
  `GetSystemInfo`, the native processor architecture of 0 (`INTEL`) was being
  interpreted as 64-bit. Now it is correctly interpreted to be 32-bit x86
  (DOTNET-1048).

#### 4.2.2. Version 0.3.1

Features:

- WinVersion: Support Windows 11 23H2 (DOTNET-1015).
- WinVersion: For unknown Windows Versions, use the Windows Branding string
  `%WINDOWS_LONG%` which shows Windows 11 properly (DOTNET-1015).

#### 4.2.3. Version 0.3.0

Features:

- NetVersion: Search for Mono 4.2.1 and later (DOTNET-848).
- NetVersion: Search for Mono 1.x to 4.0.3 (DOTNET-849).
- NetVersion: Use the current MonoRuntime to also determine the installed
  location (DOTNET-851).
- NetVersion: Search the path on Windows for other installations of Mono
  (DOTNET-858).
- NetVersion: Use the current MonoRuntime on Linux to also determine the
  installed location (DOTNET-863).
- WinVersion:  Add Windows RT 8.1 ARM to a test case (DOTNET-859).

BugFixes:

- NetVersion: Fix and test .NET 1.0 to .NET 4.8.1, fixing especially the
  detection of .NET 3.0 (DOTNET-843, DOTNET-844, DOTNET-846).
- NetVersion: Only return one current runtime (DOTNET-850).

Quality:

- Add `README.md` to NuGet package (DOTNET-805).
- NetVersion: Provide the Target Version as a `Version` object (DOTNET-842,
  DOTNET-845).
- Code cleanup (DOTNET-848).
- Add references for OS Compatibility (DOTNET-938).
- Upgrade to .NET Core 6.0 (DOTNET-936, DOTNET-940, DOTNET-942, DOTNET-959,
  DOTNET-963).
- Add .NET 4.8 target, and enable usage of APIs without reflection (DOTNET-976).

### 4.3. Version 0.2.0

- Initial Release
