# Windows Version API Dump Tool <!-- omit in toc -->

Files in this directory are precompiled versions of the tools for various
Windows Operating Systems, to get version information.

## 1. Binaries

### 1.1. WinXP (Visual Studio 2005 SP1 x86, x64)

Precompiled binaries for Windows XP, compiled with Visual Studio 2005 SP1.

These will work on Windows NT4 SP6a, Windows 2000 SP4 up to Windows 11. These
images have no manifest present (so we can see the effect this has on
`GetVersionEx` reporting 6.2.9200 instead of 10.0.x).

You will need to have the Microsoft Visual C++ 2005 Redistributable Packages
installed, else you might get the error:

> The application has failed to start because its side-by-side configuration is
> incorrect. Please see the application event log for more detail.

### 1.2. Win10 (Visual Studio 2022 17.14.2 x86, x64)

Precompiled binaries for Windows 10, 11, compiled with Visual Studio 2022
(17.14.2). They contain a manifest declaring compatibility with Windows Vista,
7, 8, 8.1 and Windows 10/11.

To get the binaries running properly on Vista, you'll need to find an older
version of the Microsoft Visual C++ 2015-2019 runtime installation packages. You
may need to find in addition libraries (`api-ms-win-core-*.dll`) and put them
along side the executable for it to work properly with the runtime library.
