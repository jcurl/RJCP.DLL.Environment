# Windows Version API Dump Tool <!-- omit in toc -->

Files in this directory are precompiled versions of the tools for various
Windows Operating Systems, to get version information.

## 1. Binaries

### 1.1. Windows 9x (Visual Studio 98)

Precompiled binaries for Windows 9x, compiled with Visual Studio 98.

These will work on Windows 95a, NT 3.51, 4, etc. up to Windows 11. These images
have no manifest present (which wasn't invented at this time), so they run in
compatibility mode on modern Windows.

They are compiled for MBCS (Multi-Byte Character Support) so they run on Windows
9x. For Windows NT, conversions from Unicode to MBCS is made as necessary to
output some strings.

### 1.2. WinXP (Visual Studio 2005 SP1 x86, x64)

Precompiled binaries for Windows XP, compiled with Visual Studio 2005 SP1.

These will work on Windows NT 3.51, 4.0 SP1, Windows 2000 SP4, etc. up to
Windows 11. These images have no manifest present (so we can see the effect this
has on `GetVersionEx` reporting 6.2.9200 instead of 10.0.x).

You will need to have the Microsoft Visual C++ 2005 Redistributable Packages
installed, else you might get the error:

> The application has failed to start because its side-by-side configuration is
> incorrect. Please see the application event log for more detail.

### 1.3. Windows 8.1 (Visual Studio 2012 Update 5)

Using the Windows 10 binaries from Visual Studio 2022 works fine when older
Operating Systems are fully patched with the latest service pack, but this makes
it difficult with Windows Vista RTM and Windows 7 RTM.

These are built using Visual Studio 2012 on Windows 7 64-bit. The toolchain
chosen is the "v110_xp" to allow the binaries to also run on Windows XP SP3.
Note, it will not run on earlier versions due to a missing `DecodePointer`
kernel32.dll function which is missing.

Make sure you have the Microsoft Visual C++ 2012 Redistributable Packages
installed. These will also install on Windows XP SP3.

### 1.4. Win10 (Visual Studio 2022 17.14.2 x86, x64)

Precompiled binaries for Windows 10, 11, compiled with Visual Studio 2022
(17.14.2). They contain a manifest declaring compatibility with Windows Vista,
7, 8, 8.1 and Windows 10/11.

To get the binaries running properly on Vista, you'll need to find an older
version of the Microsoft Visual C++ 2015-2019 runtime installation packages. You
may need to find in addition libraries (`api-ms-win-core-*.dll`) and put them
along side the executable for it to work properly with the runtime library.
