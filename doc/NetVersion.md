# Obtaining .NET Framework Versions <!-- omit in toc -->

This page contains references to information and experiments that are useful for
the design and implementation of the NetVersions class in this repository.

- [1. Prior Art](#1-prior-art)
- [2. Detecting DotNET Framework Installations on Windows](#2-detecting-dotnet-framework-installations-on-windows)
  - [2.1. DotNET Framework 4.5 to 4.8.1](#21-dotnet-framework-45-to-481)
    - [2.1.1. Mapping of Release Table](#211-mapping-of-release-table)
    - [2.1.2. CLR Installation Version DotNET 4.5 to 4.8.1](#212-clr-installation-version-dotnet-45-to-481)
  - [2.2. DotNET Framework 1.1 to 4.0](#22-dotnet-framework-11-to-40)
    - [2.2.1. DotNET Framework 3.0](#221-dotnet-framework-30)
    - [2.2.2. CLR Installation Version DotNET 1.1 to 4.0](#222-clr-installation-version-dotnet-11-to-40)
  - [2.3. DotNET Framework 1.0](#23-dotnet-framework-10)
    - [2.3.1. CLR Installation Version DotNET 1.0](#231-clr-installation-version-dotnet-10)
- [3. Detecting Mono on Windows](#3-detecting-mono-on-windows)
  - [3.1. Mono 1.2.1 - 3.2.3](#31-mono-121---323)
  - [3.2. Mono 3.12.0 - 4.0.3](#32-mono-3120---403)
  - [3.3. Mono 4.2.1 - 6.12](#33-mono-421---612)
  - [3.4. Finding the Profiles](#34-finding-the-profiles)
  - [3.5. API Versions](#35-api-versions)
- [4. Testing](#4-testing)
- [5. Sample Registry Values](#5-sample-registry-values)
  - [5.1. Windows 10 with DotNET 4.8.1 installed](#51-windows-10-with-dotnet-481-installed)
  - [5.2. Windows XP with only DotNET 1.0 installed](#52-windows-xp-with-only-dotnet-10-installed)
  - [5.3. Windows XP with only DotNET 3.0 Installed](#53-windows-xp-with-only-dotnet-30-installed)
  - [5.4. Windows XP with only DotNET 3.5 installed](#54-windows-xp-with-only-dotnet-35-installed)
  - [5.5. Windows XP with all DotNET Frameworks Installed](#55-windows-xp-with-all-dotnet-frameworks-installed)

## 1. Prior Art

Microsoft has the page [How to Determine which Versions are
Installed](https://learn.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed).
This is the basis for the implementation.

There is
[NetFrameworkLocator.cs](https://github.com/nunit/nunit-console/blob/3.16.3/src/NUnitEngine/nunit.engine.core/Internal/RuntimeFrameworks/NetFrameworkLocator.cs)
from the [NUnit Console Runner](https://github.com/nunit/nunit-console).

## 2. Detecting DotNET Framework Installations on Windows

The base key for all registry entries is
`HKEY_LOCAL_MACHINE\Software\Microsoft`. The following sections show the path
relative to here.

### 2.1. DotNET Framework 4.5 to 4.8.1

The version of .NET Framework (4.5 and later) installed on a machine is listed
in the registry at `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework
Setup\NDP\v4\Full`. If the registry key is missing, then .NET 4.5 or later is
not installed.

#### 2.1.1. Mapping of Release Table

| .NET Framework version | value  | Installation                     |
| ---------------------- | ------ | -------------------------------- |
| .NET Framework 4.5     | 378389 |                                  |
| .NET Framework 4.5.1   | 378675 | Windows 8.1                      |
|                        | 378758 | Installer                        |
| .NET Framework 4.5.2   | 379893 |                                  |
| .NET Framework 4.6     | 393295 | Windows 10                       |
|                        | 393297 | Installer                        |
| .NET Framework 4.6.1   | 394254 | Windows 10                       |
|                        | 394271 | Installer                        |
| .NET Framework 4.6.2   | 394802 | Windows 10                       |
|                        | 394806 | Installer                        |
| .NET Framework 4.7     | 460798 | Windows 10                       |
|                        | 460805 | Installer                        |
| .NET Framework 4.7.1   | 461308 | Windows 10                       |
|                        | 461310 | Installer                        |
| .NET Framework 4.7.2   | 461808 | Windows 10                       |
|                        | 461814 | Installer                        |
| .NET Framework 4.8     | 528040 | Windows 10 1903                  |
|                        | 528049 | Installer                        |
|                        | 528209 | Windows 10 1909                  |
|                        | 528372 | Windows 10 20H2                  |
|                        | 528449 | Windows 11 21H2                  |
| .NET Framework 4.8.1   | 533320 | Windows 10 22H2, Windows 11 22H2 |
|                        | 533325 | Installer                        |

#### 2.1.2. CLR Installation Version DotNET 4.5 to 4.8.1

To get the Install Version, query `NET Framework Setup\NDP\v4\Full` with key
`Version` (`REG_SZ`).

### 2.2. DotNET Framework 1.1 to 4.0

.NET runtime may also be 32-bit only, and registry keys should be checked via
the WoW6432 node `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\`

| Framework Version | Profile | Subkey                                                       | Key              | Type        | Value |
| ----------------- | ------- | ------------------------------------------------------------ | ---------------- | ----------- | ----- |
| 1.1               |         | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v1.1.4322`  | `Install`        | `REG_DWORD` | `1`   |
| 2.0               |         | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v2.0.50727` | `Install`        | `REG_DWORD` | `1`   |
| 3.0               |         | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v3.0\Setup` | `InstallSuccess` | `REG_DWORD` | `1`   |
| 3.5               |         | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v3.5`       | `Install`        | `REG_DWORD` | `1`   |
| 4.0               | Client  | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v4\Client`  | `Install`        | `REG_DWORD` | `1`   |
| 4.0               | Full    | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v4\Full`    | `Install`        | `REG_DWORD` | `1`   |

#### 2.2.1. DotNET Framework 3.0

Note, with an installation of .NET 3.5, the following key is also present.

| Framework Version | Profile | Subkey                                                 | Key       | Type        | Value |
| ----------------- | ------- | ------------------------------------------------------ | --------- | ----------- | ----- |
| 3.0               |         | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v3.0` | `Install` | `REG_DWORD` | `1`   |

However, with a fresh install of Windows XP and .NET 3.0 only, the above key
will *not* be found. One must look at:

| Framework Version | Profile | Subkey                                                       | Key              | Type        | Value |
| ----------------- | ------- | ------------------------------------------------------------ | ---------------- | ----------- | ----- |
| 3.0               |         | `HKLM\Software\Microsoft\NET Framework Setup\NDP\v3.0\Setup` | `InstallSuccess` | `REG_DWORD` | `1`   |

#### 2.2.2. CLR Installation Version DotNET 1.1 to 4.0

A generic algorithm to get the installation version (note, not the framework
version):

- When iterating over the key `HKLM\Software\Microsoft\NET Framework
  Setup\NDP\v{0}`, the version part in the key should be used first.
  - The major and minor is the framework version.
- If the `Install` value is not in the key, then ignore. Or if the key itself
  has the value `deprecated` then ignore.
- If there is no `Version` key, then use the NDP key, else read the string and
  use that version. Note, that this may appear different to the .NET framework
  version. For example, on Windows XP with .NET 3.0 and service packs installed,
  it could be `3.2.x` and not `3.0`.
- If there is an `Increment` key, this can be the fourth element in the version.
- Get the Service Pack version with the key `SP` if it exists. A value of zero
  indicates no service pack update.

For Framework 3.0, the following should also be done, if it is not iterated
above:

- Check the `Setup` key if it exists. If the `InstallSuccess` value is `1`, then
  get the `Version`.

### 2.3. DotNET Framework 1.0

The Microsoft documentation at [Detect .NET Framework 1.0 through
4.0](https://learn.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#detect-net-framework-10-through-40)
does not work for .NET 1.0 installed on Windows XP (see below for a sample
registry export)

| Framework Version | Profile | Subkey                                                   | Key       | Type     | Value |
| ----------------- | ------- | -------------------------------------------------------- | --------- | -------- | ----- |
| 1.0               |         | `HKLM\Software\Microsoft\.NETFramework\Policy\v1.0\3705` | `Install` | `REG_SZ` | `1`   |

There is no `Install` key.

Follow these steps:

- Open the subkey `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework
  Setup\Full`
- Iterate over the subkeys that start with `v`, like `v1.0.3705`
- Iterate over the subkeys which are the language codes, like `1033`
- Iterate over the subkeys which is the product name
- Open `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\Product\{0}`
  where that key is the product name
- Ensure that the key `Install` is 1.
- If the `Package` is `Full` and `ProductLanguage` matches the language code
  then this is installed.

Note, there is no overlap with this algorithm for .NET 4.0 and later, as this
has the key name `NET Framework Setup\NDP`.

#### 2.3.1. CLR Installation Version DotNET 1.0

When following the steps above, additionally get the version from the `Version`
value from the product key (the last step).

## 3. Detecting Mono on Windows

Various versions of Mono were installed on a 64-bit version of Windows. This can
help determine the behaviour for detection. The base path for the table is
`HKEY_LOCAL_MACHINE\SOFTWARE`.

### 3.1. Mono 1.2.1 - 3.2.3

Mono 3.0.1 installs in a project name specific directory.

They keys are in `HKLM\SOFTWARE\Novell`

| Version | Key Path                 | Key                          | Type      | Value                                   |
| ------- | ------------------------ | ---------------------------- | --------- | --------------------------------------- |
| 3.0.1   | `Novell`                 |                              |           |                                         |
|         | `Novell/Mono`            |                              |           |                                         |
|         |                          | `DefaultCLR`                 | REG_SZ    | 3.0.1                                   |
|         | `Novell/Mono/3.0.1`      |                              |           |                                         |
|         |                          | `FrameworkAssemblyDirectory` | REG_SZ    | `C:\Program Files (x86)\Mono-3.0.1\lib` |
|         |                          | `GtkPlusDevIsInstalled`      | REG_DWORD | 1                                       |
|         |                          | `GtkSharpIsInstalled`        | REG_DWORD | 1                                       |
|         |                          | `MonoConfigDir`              | REG_SZ    | `C:\Program Files (x86)\Mono-3.0.1\etc` |
|         |                          | `SdkInstallRoot`             | REG_SZ    | `C:\Program Files (x86)\Mono-3.0.1`     |
|         |                          | `XSPIsInstalled`             | REG_DWORD | 1                                       |
|         | `Novell/Mono DefaultCLR` |                              |           |                                         |
|         |                          | `Installer Language`         | REG_SZ    | `1033`                                  |

The only installers are available are 32-bit, so the architecture is `x86`.

Multiple installations of the Mono Runtime can be installed. That is, 3.0.1 and
3.0.2 can be installed at the same time. The key `DefaultCLR` is updated for the
last installed version.

If the user installs version A, followed by B, then uninstalls B, the
`DefaultCLR` registry key is also removed.

### 3.2. Mono 3.12.0 - 4.0.3

Mono 3.12.0 to 4.0.3 does not install any registry keys, so the only way to
determine if something is installed is by looking in the default program folders
file for 32-bit. These installers are MSI based, where the previous 3.x
installers were executables.

Version 4.0.0 and later have File Version information on `mscorlib.dll`, but
this is the version of the framework it is emulating, not the version of the
Mono runtime. The `mono.exe` binary has no version information. This could be
helpful in providing the framework version, but not the install version.

### 3.3. Mono 4.2.1 - 6.12

| Version    | Key Path | Key                          | Type      | Value                        |
| ---------- | -------- | ---------------------------- | --------- | ---------------------------- |
| 6.12.0.200 | `Mono`   |                              |           |                              |
|            |          | `Architecture`               | REG_SZ    | `x64`                        |
|            |          | `FrameworkAssemblyDirectory` | REG_SZ    | `C:\Program Files\Mono\lib\` |
|            |          | `Installed`                  | REG_DWORD | 1                            |
|            |          | `MonoConfigDir`              | REG_SZ    | `C:\Program Files\Mono\etc\` |
|            |          | `SdkInstallRoot`             | REG_SZ    | `C:\Program Files\Mono\`     |
|            |          | `Version`                    | REG_SZ    | `6.12.0.200`                 |

Because it is possible to have the 32-bit and 64-bit versions of Mono installed
at the same time, an application should run as 64-bit, and also check the
`Wow6432Node`:

| Version    | Key Path | Key                          | Type      | Value                              |
| ---------- | -------- | ---------------------------- | --------- | ---------------------------------- |
| 6.12.0.200 | `Mono`   |                              |           |                                    |
|            |          | `Architecture`               | REG_SZ    | `x86`                              |
|            |          | `FrameworkAssemblyDirectory` | REG_SZ    | `C:\Program Files (x86)\Mono\lib\` |
|            |          | `Installed`                  | REG_DWORD | 1                                  |
|            |          | `MonoConfigDir`              | REG_SZ    | `C:\Program Files (x86)\Mono\etc\` |
|            |          | `SdkInstallRoot`             | REG_SZ    | `C:\Program Files (x86)\Mono\`     |
|            |          | `Version`                    | REG_SZ    | `6.12.0.200`                       |

If the `Architecture` key is not present, assume `x86`. It was first added in
Mono 4.4.0.

### 3.4. Finding the Profiles

[RuntimeFramework.cs::FindAllMonoProfiles](https://github.com/nunit/nunit-console/blob/3.16.3/src/NUnitEngine/nunit.engine.core/RuntimeFramework.cs#L629)
searches for the profiles in the following order:

- Get the `MonoPrefix` which is the key `SdkInstallRoot`.
- If the file `lib/mono/1.0/mscorlib.dll` exists, add version 1.1.4322
- If the file `lib/mono/2.0/mscorlib.dll` exists, add version 2.0
- If the directory `lib/mono/3.5` exists, add version 3.5
- If the file `lib/mono/4.0/mscorlib.dll` exists, add version 4.0
- If the file `lib/mono/4.5/mscorlib.dll` exists, add version 4.5

In general, if the file `mscorelib.dll` exists on Windows, the detection library
should get the version of the file. Not every copy has a file version. The
assembly or the library should *not* be loaded into memory space which could be
a security vulnerability.

### 3.5. API Versions

There isn't a reliable way to know the .NET API version. But as this library
detects runtimes, this is irrelevant. The version of the run time is what
defines the APIs that are implemented. The API versions are only needed for
compiling software. Here's a summary of testing by installing on Windows 7.

Observed are various API folders in the
`<FrameworkAssemblyDirectory>\mono\xxx-api` where `xxx` is in the table below in
the column *Framework API Folders*.

| Version      | Framework Folders  | Framework API Folders | Notes                                 |
| ------------ | ------------------ | --------------------- | ------------------------------------- |
| 1.2          | 1.0, 2.0           | -                     |                                       |
| 1.2.1        | 1.0, 2.0           | -                     |                                       |
| 1.2.2        | 1.0, 2.0           | -                     |                                       |
| 1.2.2.1      | 1.0, 2.0           | -                     |                                       |
| 1.2.3        | 1.0, 2.0           | -                     |                                       |
| 1.2.3.1      | 1.0, 2.0           | -                     |                                       |
| 1.2.3.50     | 1.0, 2.0           | -                     |                                       |
| 1.2.4        | 1.0, 2.0           | -                     |                                       |
| 1.2.5        | 1.0, 2.0           | -                     |                                       |
| 1.2.5.1      | 1.0, 2.0           | -                     |                                       |
| 1.2.5.2      | 1.0, 2.0           | -                     |                                       |
| 1.2.6        | 1.0, 2.0, 2.1      | -                     |                                       |
| 1.9          | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 1.9.1        | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.0          | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.0.1        | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.2          | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.4          | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.4.2        | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.4.2.1      | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.4.2.2      | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.4.2.3      | 1.0, 2.0, 2.1, 3.5 | -                     |                                       |
| 2.4.3        | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.4.3.1      | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.6          | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.6.1        | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.6.3        | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.6.4        | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.6.7        | 1.0, 2.0, 3.5      | -                     |                                       |
| 2.8          | 2.0, 3.5, 4.0      | -                     | Doesn't have Is64BitOperatingSystem   |
| 2.8.1        | 2.0, 3.5, 4.0      | -                     | Doesn't have Is64BitOperatingSystem   |
| 2.8.2        | 2.0, 3.5, 4.0      | -                     | Doesn't have Is64BitOperatingSystem   |
| 2.10         | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.1       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.2       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.3       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.4       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.5       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.6       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.10.9       | 2.0, 3.5, 4.0      | -                     |                                       |
| 2.11         | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 2.11.2       | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 2.11.3       | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 2.11.4       | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.1        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.2        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.3        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.4        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.5        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.6        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.8        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.9        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.0.10       | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.2.3        | 2.0, 3.5, 4.0, 4.5 | -                     |                                       |
| 3.12.0       | 2.0, 3.5, 4.0, 4.5 | -                     | No registry keys                      |
| 3.12.1       | 2.0, 3.5, 4.0, 4.5 | -                     | No registry keys                      |
| 4.0.0 alpha1 | 4.0, 4.5           | -                     | No registry keys                      |
| 4.0.1        | 2.0, 3.5, 4.0, 4.5 | -                     | No registry keys                      |
| 4.0.2        | 2.0, 3.5, 4.0, 4.5 | -                     | No registry keys                      |
| 4.0.3        | 2.0, 3.5, 4.0, 4.5 | -                     | No registry keys.                     |
| 4.2.1.124    | 2.0, 3.5, 4.0, 4.5 | -                     | The `Version` key has no build number |
| 4.2.2.30     | 2.0, 3.5, 4.0, 4.5 | -                     | The `Version` key has no build number |
| 4.2.3.4      | 2.0, 3.5, 4.0, 4.5 | -                     | The `Version` key has no build number |
| 4.2.4.4      | 2.0, 3.5, 4.0, 4.5 | -                     | The `Version` key has no build number |
| 4.3.2.467    | 4.5                | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.4.0.182    | 4.0, 4.5           | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.4.1.0      | 4.0, 4.5           | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.4.2.11     | 4.0, 4.5           | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.6.0.245    | 4.5                | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.6.1.5      | 4.0, 4.5           | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.6.2.16     | 4.5                | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.8.0.524    | 4.0, 4.5           | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 4.8.1.0      | 4.0, 4.5           | 2.0, 3.5, 4.0, 4.5    | The `Version` key has no build number |
| 5.0.0.100    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.6.2 | The `Version` key has no build number |
| 5.0.1.1      | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.6.2 | The `Version` key has no build number |
| 5.2.0.224    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7   | The `Version` key has no build number |
| 5.4.0.201    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7   | The `Version` key has no build number |
| 5.4.1.7      | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7   | The `Version` key has no build number |
| 5.8.0.127    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 | The `Version` key has no build number |
| 5.8.1.0      | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 | The `Version` key has no build number |
| 5.10.0.179   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 |                                       |
| 5.10.1.57    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 |                                       |
| 5.12.0.301   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 |                                       |
| 5.14.0.177   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 |                                       |
| 5.16.0.220   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 |                                       |
| 5.16.1.0     | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.1 |                                       |
| 5.18.0.268   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.2 |                                       |
| 5.18.1.28    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.2 |                                       |
| 5.20.0.244   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.2 |                                       |
| 5.20.1.24    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.2 |                                       |
| 6.0.0.34     | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.2 |                                       |
| 6.4.0.199    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.7.2 |                                       |
| 6.6.0.166    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.8   |                                       |
| 6.8.0.123    | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.8   |                                       |
| 6.10.0.103   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.8   |                                       |
| 6.12.0.200   | 4.0, 4.5           | 2.0, 3.5, 4.0 - 4.8   |                                       |

What's important in this table to note, is that while Mono supports various
APIs, not all profiles are visible. This can affect how applications search for
available runtimes.

## 4. Testing

On Windows XP, it is possible to have .NET 1.0 and later installed, and still
run this program, if the Mono runtime is used to run the software. The latest
installable version of Mono on Windows XP is
[3.2.3](https://download.mono-project.com/archive/3.2.3/windows-installer/).

## 5. Sample Registry Values

This section helps to provide actual samples after installation for software
checking. Various different software checks different keys. Different Operating
Systems have different keys and values depending on what is installed. The
tables help to identify differences for testing.

### 5.1. Windows 10 with DotNET 4.8.1 installed

Windows 10 with .NET 4.8.1 installed. There are many more keys than mentioned
here, those that are related to the installation version are shown. A complete
dump can be found in the repository.

| Key Path                                                                  | Key                               | Type      | Value                                                                          |
| ------------------------------------------------------------------------- | --------------------------------- | --------- | ------------------------------------------------------------------------------ |
| .NETFramework                                                             |                                   |           |                                                                                |
|                                                                           | DbjJITDebugLaunchSetting          | REG_DWORD | 0x00000010                                                                     |
|                                                                           | DbgManagedDebugger                | REG_SZ    | "C:\WINDOWS\system32\vsjitdebugger.exe" PID %d APPDOM %d EXTEXT "%s" EVTHDL %d |
|                                                                           | Enable64Bit                       | REG_DWORD | 0x00000001                                                                     |
|                                                                           | InstallRoot                       | REG_SZ    | C:\Windows\Microsoft.NET\Framework64\                                          |
|                                                                           | UseRyuJIT                         | REG_DWORD | 0x00000001                                                                     |
| .NETFramework\policy                                                      |                                   |           |                                                                                |
| .NETFramework\policy\Upgrades                                             |                                   |           |                                                                                |
|                                                                           | 2.0.50727                         | REG_SZ    | 1.0.0-2.0.50727                                                                |
|                                                                           | 4.0.30319                         | REG_SZ    | 3.0.0-4.0.30319                                                                |
| .NETFramework\policy\v2.0                                                 |                                   |           |                                                                                |
|                                                                           | 50727                             | REG_SZ    | 50727-50727                                                                    |
| .NETFramework\policy\v4.0                                                 |                                   |           |                                                                                |
|                                                                           | 30319                             | REG_SZ    | 30319-30319                                                                    |
| .NETFramework\v2.0.50727                                                  |                                   |           |                                                                                |
|                                                                           | AspNetEnforceViewStateMac         | REG_DWORD | 1                                                                              |
| .NETFramework\v3.0                                                        |                                   |           |                                                                                |
| .NETFramework\v4.0.30319                                                  |                                   |           |                                                                                |
|                                                                           | AspNetEnforceViewStateMac         | REG_DWORD | 1                                                                              |
| .NETFramework\v4.0.30319\SKUs                                             |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0                  |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0,Profile=Client   |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.1                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.1,Profile=Client |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.2                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.2,Profile=Client |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.3                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.3,Profile=Client |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.5                  |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.5.1                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.5.2                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.5.3                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.6                  |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.6.1                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.6.2                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.7                  |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.7.1                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.7.2                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.8                  |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.8.1                |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\Client                                      |                                   |           |                                                                                |
| .NETFramework\v4.0.30319\SKUs\Default                                     |                                   |           |                                                                                |
| NET Framework Setup\NDP\CDF                                               |                                   |           |                                                                                |
| NET Framework Setup\NDP\CDF\v4.0                                          |                                   |           |                                                                                |
|                                                                           | HttpNamespaceReservationInstalled | REG_DWORD | 1                                                                              |
|                                                                           | NetTcpPortSharingInstalled        | REG_DWORD | 1                                                                              |
|                                                                           | NonHttpActivationInstalled        | REG_DWORD | 1                                                                              |
|                                                                           | SMSvcHostPath                     | REG_SZ    | C:\Windows\Microsoft.NET\Framework64\v4.0.30319\                               |
|                                                                           | WMIInstalled                      | REG_DWORD | 1                                                                              |
| NET Framework Setup\NDP\v2.0.50727                                        |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              |
|                                                                           | Increment                         | REG_SZ    | 4927                                                                           |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | OCM                               | REG_DWORD | 1                                                                              |
|                                                                           | SP                                | REG_DWORD | 2                                                                              |
|                                                                           | Version                           | REG_SZ    | 2.0.50727.4927                                                                 |
| NET Framework Setup\NDP\v2.0.50727\1033                                   |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              |
|                                                                           | Increment                         | REG_SZ    | 4927                                                                           |
|                                                                           | SP                                | REG_DWORD | 2                                                                              |
|                                                                           | Version                           | REG_SZ    | 2.0.50727.4927                                                                 |
| NET Framework Setup\NDP\v2.0.50727\1031                                   |                                   |           |                                                                                |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                                                              |
|                                                                           | OCM                               | REG_DWORD | 1                                                                              |
| NET Framework Setup\NDP\v3.0                                              |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              |
|                                                                           | Increment                         | REG_SZ    | 4926                                                                           |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | SP                                | REG_DWORD | 2                                                                              |
|                                                                           | Version                           | REG_SZ    | 3.0.30729.4926                                                                 |
| NET Framework Setup\NDP\v3.5                                              |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | InstallPath                       | REG_SZ    | C:\Windows\Microsoft.NET\Framework64\v3.5\                                     |
|                                                                           | SP                                | REG_DWORD | 1                                                                              |
|                                                                           | Version                           | REG_SZ    | 3.5.30729.4926                                                                 |
| NET Framework Setup\NDP\v4                                                |                                   |           |                                                                                |
| NET Framework Setup\NDP\v4\Full                                           |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | InstallPath                       | REG_SZ    | C:\Windows\Microsoft.NET\Framework64\v4.0.30319\                               |
|                                                                           | Release                           | REG_DWORD | 533325                                                                         |
|                                                                           | Servicing                         | REG_DWORD | 0                                                                              |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                                                          |
|                                                                           | Version                           | REG_SZ    | 4.8.09037                                                                      |
| NET Framework Setup\NDP\v4\Full\1033                                      |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              | 1 |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | Release                           | REG_DWORD | 533325                                                                         |
|                                                                           | Servicing                         | REG_DWORD | 0                                                                              |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                                                          |
|                                                                           | Version                           | REG_SZ    | 4.8.09037                                                                      |
| NET Framework Setup\NDP\v4\Client                                         |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | InstallPath                       | REG_SZ    | C:\Windows\Microsoft.NET\Framework64\v4.0.30319\                               |
|                                                                           | Release                           | REG_DWORD | 533325                                                                         |
|                                                                           | Servicing                         | REG_DWORD | 0                                                                              |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                                                          |
|                                                                           | Version                           | REG_SZ    | 4.8.09037                                                                      |
| NET Framework Setup\NDP\v4\Client\1033                                    |                                   |           |                                                                                |
|                                                                           | CBS                               | REG_DWORD | 1                                                                              | 1 |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | Release                           | REG_DWORD | 533325                                                                         |
|                                                                           | Servicing                         | REG_DWORD | 0                                                                              |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                                                          |
|                                                                           | Version                           | REG_SZ    | 4.8.09037                                                                      |
| NET Framework Setup\NDP\v4.0                                              |                                   | REG_SZ    | deprecated                                                                     |
| NET Framework Setup\NDP\v4.0\Client                                       |                                   |           |                                                                                |
|                                                                           | Install                           | REG_DWORD | 1                                                                              |
|                                                                           | Version                           | REG_SZ    | 4.0.0.0                                                                        |

### 5.2. Windows XP with only DotNET 1.0 installed

| Key Path                                                                               | Key              | Type      | Value                                                                   |
| -------------------------------------------------------------------------------------- | ---------------- | --------- | ----------------------------------------------------------------------- |
| .NETFramework                                                                          |                  |           |                                                                         |
|                                                                                        | InstallRoot      | REG_SZ    | C:\WINDOWS\Microsoft.NET\Framework\                                     |
| .NETFramework\Dummy                                                                    |                  |           |                                                                         |
|                                                                                        | FXCONFIGSHORTCUT | REG_SZ    | 1                                                                       |
| .NETFramework\PendingUpdates                                                           |                  |           |                                                                         |
| .NETFramework\PendingUpdates\v1.0.3705                                                 |                  |           |                                                                         |
| .NETFramework\PendingUpdates\v1.0.3705\NI                                              |                  | REG_SZ    | "C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705\ngen.exe" /nologo /silent |
| .NETFramework\policy                                                                   |                  |           |                                                                         |
| .NETFramework\policy\v1.0                                                              |                  |           |                                                                         |
|                                                                                        | 3705             | REG_SZ    | 3321-3705                                                               |
| NET Framework Setup                                                                    |                  |           |                                                                         |
| NET Framework Setup\Full                                                               |                  |           |                                                                         |
| NET Framework Setup\Full\v1.0.3705                                                     |                  |           |                                                                         |
| NET Framework Setup\Full\v1.0.3705\1033                                                |                  |           |                                                                         |
|                                                                                        | ProductCode      | REG_SZ    | {B43357AA-3A6D-4D94-B56E-43C44D09E548}                                  |
| NET Framework Setup\Full\v1.0.3705\1033\Microsoft .NET Framework Full v1.0.3705 (1033) | Install          | REG_DWORD | 1                                                                       |
| NET Framework Setup\Product                                                            |                  |           |                                                                         |
| NET Framework Setup\Product\Microsoft .NET Framework Full v1.0.3705 (1033)             |                  |           |                                                                         |
|                                                                                        | Package          | REG_SZ    | Full                                                                    |
|                                                                                        | Version          | REG_SZ    | v1.0.3705                                                               |
|                                                                                        | ProductLanguage  | REG_SZ    | 1033                                                                    |

### 5.3. Windows XP with only DotNET 3.0 Installed

A fresh version of Windows XP SP3 was installed with .NET 3.0

| Key Path                                | Key            | Type      | Value                               |
| --------------------------------------- | -------------- | --------- | ----------------------------------- |
| .NETFramework                           |                |           |                                     |
|                                         | InstallRoot    | REG_SZ    | C:\WINDOWS\Microsoft.NET\Framework\ |
| .NETFramework\Policy                    |                |           |                                     |
| .NETFramework\Policy\Upgrades           |                |           |                                     |
|                                         | 2.0.50727      | REG_SZ    | 1.0.0-2.0.50727                     |
| .NETFramework\Policy\v2.0               |                |           |                                     |
|                                         | 50727          | REG_SZ    | 50727-50727                         |
| .NETFramework\v2.0.50727                |                |           |                                     |
| .NETFramework\v2.0.50727\SBSDisabled    |                |           |                                     |
|                                         | Uninstall      | REG_SZ    | 1                                   |
| .NETFramework\v3.0                      |                |           |                                     |
| NET Framework Setup                     |                |           |                                     |
| NET Framework Setup\NDP                 |                |           |                                     |
| NET Framework Setup\NDP\v2.0.50727      |                |           |                                     |
|                                         | Install        | REG_DWORD | 1                                   |
|                                         | Increment      | REG_SZ    | 42                                  |
|                                         | MSI            | REG_DWORD | 1                                   |
|                                         | SP             | REG_DWORD | 0                                   |
| NET Framework Setup\NDP\v2.0.50727\1033 |                |           |                                     |
|                                         | Install        | REG_DWORD | 1                                   |
|                                         | MSI            | REG_DWORD | 1                                   |
|                                         | SP             | REG_DWORD | 0                                   |
| NET Framework Setup\NDP\v3.0            |                |           |                                     |
| NET Framework Setup\NDP\v3.0\Setup      |                |           |                                     |
|                                         | InstallSuccess | REG_DWORD | 1                                   |
|                                         | Version        | REG_SZ    | 3.0.04506.30                        |

We see with the available keys, that detecting .NET 3.0 is different in that
there is no `Install` key. In the next section, we see installing .NET 3.5 only
adds this key and corrects the difference.

### 5.4. Windows XP with only DotNET 3.5 installed

A fresh version of Windows XP SP3 was installed with .NET 3.5

| Key Path                                | Key            | Type      | Value                                    |
| --------------------------------------- | -------------- | --------- | ---------------------------------------- |
| .NETFramework                           |                |           |                                          |
|                                         | InstallRoot    | REG_SZ    | c:\WINDOWS\Microsoft.NET\Framework\      |
| .NETFramework\Policy                    |                |           |                                          |
| .NETFramework\Policy\Upgrades           |                |           |                                          |
|                                         | 2.0.50727      | REG_SZ    | 1.0.0-2.0.50727                          |
| .NETFramework\Policy\v2.0               |                |           |                                          |
|                                         | 50727          | REG_SZ    | 50727-50727                              |
| .NETFramework\v2.0 SP1                  |                |           |                                          |
| .NETFramework\v2.0 SP1\SBSDisabled      |                |           |                                          |
|                                         | Uninstall      | REG_DWORD | 1                                        |
| .NETFramework\v2.0.50727                |                |           |                                          |
| .NETFramework\v2.0.50727\SBSDisabled    |                |           |                                          |
|                                         | Install        | REG_SZ    | 1                                        |
| .NETFramework\v3.0                      |                |           |                                          |
| .NETFramework\v3.0 SP1                  |                |           |                                          |
| .NETFramework\v3.0 SP1\SBSDisabled      |                |           |                                          |
|                                         | Uninstall      | REG_DWORD | 1                                        |
| NET Framework Setup                     |                |           |                                          |
| NET Framework Setup\NDP                 |                |           |                                          |
| NET Framework Setup\NDP\v2.0.50727      |                |           |                                          |
|                                         | Install        | REG_DWORD | 1                                        |
|                                         | Version        | REG_SZ    | 2.1.21022                                |
|                                         | MSI            | REG_DWORD | 1                                        |
|                                         | Increment      | REG_SZ    | 1433                                     |
|                                         | SP             | REG_DWORD | 1                                        |
| NET Framework Setup\NDP\v2.0.50727\1033 |                |           |                                          |
|                                         | Install        | REG_DWORD | 1                                        |
|                                         | MSI            | REG_DWORD | 1                                        |
|                                         | SP             | REG_DWORD | 1                                        |
| NET Framework Setup\NDP\v3.0            |                |           |                                          |
|                                         | Install        | REG_DWORD | 1                                        |
|                                         | Version        | REG_SZ    | 3.1.21022                                |
|                                         | MSI            | REG_DWORD | 1                                        |
|                                         | Increment      | REG_SZ    |                                          |
|                                         | SP             | REG_DWORD | 1                                        |
| NET Framework Setup\NDP\v3.0\1033       |                |           |                                          |
|                                         | Install        | REG_DWORD | 1                                        |
|                                         | MSI            | REG_DWORD | 1                                        |
|                                         | SP             | REG_DWORD | 1                                        |
| NET Framework Setup\NDP\v3.0\Setup      |                |           |                                          |
|                                         | InstallSuccess | REG_DWORD | 1                                        |
|                                         | Version        | REG_SZ    | 3.1.21022                                |
| NET Framework Setup\NDP\v3.5            |                |           |                                          |
|                                         | Install        | REG_DWORD | 1                                        |
|                                         | MSI            | REG_DWORD | 1                                        |
|                                         | SP             | REG_DWORD | 0                                        |
|                                         | Version        | REG_SZ    | 3.5.21022.08                             |
|                                         | InstallPath    | REG_SZ    | C:\WINDOWS\Microsoft.NET\Framework\v3.5\ |
| NET Framework Setup\NDP\v3.5\1033       |                |           |                                          |
|                                         | Install        | REG_DWORD | 1                                        |
|                                         | MSI            | REG_DWORD | 1                                        |
|                                         | SP             | REG_DWORD | 0                                        |

We can see that .NET 3.5 now provides the `Install` key, which the default
installation of .NET 3.0 did not do.

### 5.5. Windows XP with all DotNET Frameworks Installed

Every framework, except 1.0 is installed (it won't install as 1.1 is already
installed).

| Key Path                                                                  | Key                               | Type      | Value                                          |
| ------------------------------------------------------------------------- | --------------------------------- | --------- | ---------------------------------------------- |
| .NETFramework                                                             |                                   |           |                                                |
|                                                                           | InstallRoot                       | REG_SZ    | c:\WINDOWS\Microsoft.NET\Framework\            |
| .NETFramework\Policy                                                      |                                   |           |                                                |
| .NETFramework\Policy\Upgrades                                             |                                   |           |                                                |
|                                                                           | 1.1.4322                          | REG_SZ    | 1.0.3705-1.1.4322                              |
|                                                                           | 2.0.50727                         | REG_SZ    | 1.0.0-2.0.50727                                |
|                                                                           | 4.0.30319                         | REG_SZ    | 4.0.0-4.0.30319                                |
| .NETFramework\Policy\v1.1                                                 |                                   |           |                                                |
|                                                                           | 4322                              | REG_SZ    | 3706-4322                                      |
| .NETFramework\Policy\v2.0                                                 |                                   |           |                                                |
|                                                                           | 50727                             | REG_SZ    | 50727-50727                                    |
| .NETFramework\Policy\v4.0                                                 |                                   |           |                                                |
|                                                                           | 30319                             | REG_SZ    | 30319-30319                                    |
| .NETFramework\v1.0                                                        |                                   |           |                                                |
| .NETFramework\v1.0\SBSDisabled                                            |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
| .NETFramework\v2.0 SP1                                                    |                                   |           |                                                |
| .NETFramework\v2.0 SP1\SBSDisabled                                        |                                   |           |                                                |
|                                                                           | Uninstall                         | REG_DWORD | 1                                              |
| .NETFramework\v2.0.50727                                                  |                                   |           |                                                |
| .NETFramework\v2.0.50727\SBSDisabled                                      |                                   |           |                                                |
|                                                                           | Install                           | REG_SZ    | 1                                              |
| .NETFramework\v3.0                                                        |                                   |           |                                                |
| .NETFramework\v3.0 SP1                                                    |                                   |           |                                                |
| .NETFramework\v3.0 SP1\SBSDisabled                                        |                                   |           |                                                |
|                                                                           | Uninstall                         | REG_DWORD | 1                                              |
| .NETFramework\v4.0.30319                                                  |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs                                             |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0                  |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0,Profile=Client   |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.1                |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.1,Profile=Client |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.2                |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.2,Profile=Client |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.3                |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0.3,Profile=Client |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\Client                                      |                                   |           |                                                |
| .NETFramework\v4.0.30319\SKUs\Default                                     |                                   |           |                                                |
| NET Framework Setup\NDP\CDF                                               |                                   |           |                                                |
| NET Framework Setup\NDP\CDF\v4.0                                          |                                   |           |                                                |
|                                                                           | HttpNamespaceReservationInstalled | REG_DWORD | 1                                              |
|                                                                           | NetTcpPortSharingInstalled        | REG_DWORD | 1                                              |
|                                                                           | SMSvcHostPath                     | REG_SZ    | c:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\ |
|                                                                           | WMIInstalled                      | REG_DWORD | 1                                              |
| NET Framework Setup\NDP\v1.1.4322                                         |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | SP                                | REG_DWORD | 0                                              |
| NET Framework Setup\NDP\v1.1.4322\1033                                    |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | SP                                | REG_DWORD | 0                                              |
| NET Framework Setup\NDP\v2.0.50727                                        |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | Version                           | REG_DWORD | 2.2.30729                                      |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | Increment                         | REG_SZ    | 3053                                           |
|                                                                           | SP                                | REG_DWORD | 2                                              |
| NET Framework Setup\NDP\v2.0.50727\1033                                   |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | SP                                | REG_DWORD | 2                                              |
|                                                                           | Version                           | REG_SZ    | 2.2.30729                                      |
| NET Framework Setup\NDP\v3.0                                              |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | Increment                         | REG_SZ    | 01                                             |
|                                                                           | Version                           | REG_SZ    | 3.2.30729                                      |
|                                                                           | SP                                | REG_DWORD | 2                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
| NET Framework Setup\NDP\v3.0\1033                                         |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | SP                                | REG_DWORD | 2                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | Version                           | REG_SZ    | 3.2.30729                                      |
| NET Framework Setup\NDP\v3.0\Setup                                        |                                   |           |                                                |
|                                                                           | Version                           | REG_SZ    | 3.2.30729                                      |
|                                                                           | InstallSuccess                    | REG_DWORD | 1                                              |
| NET Framework Setup\NDP\v3.5                                              |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | SP                                | REG_DWORD | 1                                              |
|                                                                           | Version                           | REG_SZ    | 3.5.30729.01                                   |
|                                                                           | InstallPath                       | REG_SZ    | c:\WINDOWS\Microsoft.NET\Framework\v3.5\       |
| NET Framework Setup\NDP\v3.5\1033                                         |                                   |           |                                                |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | SP                                | REG_DWORD | 1                                              |
|                                                                           | Version                           | REG_SZ    | 3.5.30729.01                                   |
| NET Framework Setup\NDP\v4                                                |                                   |           |                                                |
| NET Framework Setup\NDP\v4\Client                                         |                                   |           |                                                |
|                                                                           | Version                           | REG_SZ    | 4.0.30319                                      |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                          |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | Servicing                         | REG_DWORD | 0                                              |
|                                                                           | InstallPath                       | REG_SZ    | c:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\ |
| NET Framework Setup\NDP\v4\Client\1033                                    |                                   |           |                                                |
|                                                                           | Version                           | REG_SZ    | 4.0.30319                                      |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                          |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | Servicing                         | REG_DWORD | 0                                              |
| NET Framework Setup\NDP\v4\Full                                           |                                   |           |                                                |
|                                                                           | Version                           | REG_SZ    | 4.0.30319                                      |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                          |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | MSI                               | REG_DWORD | 1                                              |
|                                                                           | Servicing                         | REG_DWORD | 0                                              |
|                                                                           | InstallPath                       | REG_SZ    | c:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\ |
| NET Framework Setup\NDP\v4\Full\1033                                      |                                   |           |                                                |
|                                                                           | Version                           | REG_SZ    | 4.0.30319                                      |
|                                                                           | TargetVersion                     | REG_SZ    | 4.0.0                                          |
|                                                                           | Install                           | REG_DWORD | 1                                              |
|                                                                           | Servicing                         | REG_DWORD | 0                                              |
| NET Framework Setup\NDP\v4.0                                              |                                   | REG_SZ    | decprecated                                    |
