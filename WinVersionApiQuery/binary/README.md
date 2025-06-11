# Windows Version API Dump Tool <!-- omit in toc -->

Files in this directory are precompiled versions of the tools for various
Windows Operating Systems, to get version information.

## 1. Binaries

### 1.1. WinXP

Precompiled binaries for Windows XP SP3, compiled with Visual Studio 2022 and
the VS2017 toolchain (`Visual Studio 2017 - Windows XP (v141_xp)`).

These will work on Windows XP SP3 up to Windows 11. These images have no
manifest present (so we can see the effect this has on `GetVersionEx` reporting
6.2.9200 instead of 10.0.x).

To run the binary on Windows XP SP3, you must install vc_redist.XXX.exe version
14.27.29914.0 (from Visual Studio 2019, downloadable via
https://my.visualstudio.com).

### 1.2. Windows Vista

To use the WinXP SP3 and Win10 binaries, have Service Pack 2 installed.

On Service Pack 1 and earlier, install the MSVC/C++ runtime v14.29.30126.0 and
copy the stub DLLs. The stubs were downloaded from
[abbodie1406 v0.35.3
VisualCppRedist_AIO_x86_XP.exe](https://github.com/abbodi1406/vcredist/releases/tag/v0.35.0).
I didn't install the binary directly, but extracted them from the EXE file using
7-zip.

### 1.3. Win10

Precompiled binaries for Windows 10, 11, compiled with Visual Studio 2022
(17.14.2). They contain a manifest declaring compatibility with Windows Vista,
7, 8.0, 8.1 and Windows 10/11.
