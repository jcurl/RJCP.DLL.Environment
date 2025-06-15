# WinVersionApiQuery <!-- omit in toc -->

This tool is designed to run on the command line and call the Win32 API
reporting the results directly. It outputs the results to the console in XML
format, which can be used for comparing different Operating Systems.

## 1. Windows 10 and 11 (Visual Studio 2022 17.14.2)

The file `WinVersionApiQuery.sln` can be opened and compiled with Visual Studio
2022 (17.14.2).

### 1.1. Manifest Files

This version compiles with it a manifest file that claims compatibility with
Windows Vista, 7, 8, 8.1, 10/11. This has an effect on how the version library
routines report the Operating System version.

## 2. Windows NT4, 2000, XP and later (Visual Studio 2005 SP1 x86, x64)

The file `WinVersionApiQuery2005.sln` can be opened and copmiled with Visual
Studio 2005 SP1 (with 32-bit and 64-bit compilers).

The project is configured for Unicode, and is tested to compile on Windows XP
SP3. It runs on Windows NT 4.0 and later.

### 2.1. Windows NT 4.0 SP6

Even though Microsoft documentation claimed that binaries could be built for
Windows NT 4.0, running it would provide generate an error for the missing
`GetPathLongNameW` symbol, which is not available in Windows NT 4.0, only in
Windows XP and later.

The solution to this is given by [VS2005 and
NT4](https://web.archive.org/web/20140908080841/http://www.mombu.com/microsoft/windows-programmer-win32/t-vs2005-and-nt4-392831.html),
to statically link the runtime, which solves the problem.

To compile and test the binary on Windows NT 4.0, I installed both NT 4.0 SP6a
and XP SP3 on a virtual machine. The Windows NT 4.0 has a drive with file
sharing, which Windows XP SP3 can access via SMB1.

### 2.2. Windows NT 4.0 SP1

It is possible to install the VMWare Drivers from SP6 onto SP1 without having to
update the service pack, and without needing the MSI installer technology.

Once done, local network sharing over TCP/IP is used to share the binaries and
run on this target.

### 2.3. Windows NT 3.51 (3.51.1057.1)

I was able to install and run the same binary (`WinXP.v80`) and capture the
output on Windows NT 3.51 for x86. Network sharing is needed, in particular a
Windows NT 3.51 share should be made, and copied to that share from Windows NT
4.0 SP6.
