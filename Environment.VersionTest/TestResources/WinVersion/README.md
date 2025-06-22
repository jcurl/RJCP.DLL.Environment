# Output of Windows Version Dump Tool

Files are generally named as:

`win<major>[.<build>][-servicepack][-edition][_arch][_wow][_xp]`.

The `major` is the major version of Windows (xp, vista, 7, 8, 8.1, 10, 11, and
the server versions).

For Windows 10 and later, the `build` is the build version, highlighting the
release (e.g. 10240 is Windows 10 1507, 10586 is Windows 10 1511, etc.).

For Windows 8.1 and earlier, there may be an optional service pack.

The edition is noted (e.g. pro, ent, home, std, edu, etc.). The `ad-std`
indicates Windows Server Standard configured as the Active Directory role.

The `arch` is either `x86` or `x64` (potentially also `aarch64`). I don't have
ARM to test with.

The `wow` is appended, if a 32-bit Intel binary
`WinXX/x86/WinVersionApiQuery.exe` is run on a 64-bit Intel architecture.

The `xp` is appended, if the binary `WinXP/xXX/WinVersionApiQuery.exe` is run,
which has no manifest file (for Vista, 7, 8, 8.1, 10/11). This causes a change
in the way Windows reports versions for newer Windows.

To get the binaries working on various versions of Windows (especially the older
ones, such as Windows Vista, or Windows XP), you'll need to install the MSVC
runtime, e.g. 14.27.29914.0. Windows Vista SP1 needs stub DLLs also copied over.

## Notes on Different Operating Systems

| API Support           | `GetNativeSystemInfo` | `GetSystemInfo` | `GetVersion` | `GetVersionEx` | `RtlGetVersion` | `IsWow64Process` | `IsWow64Process2` | `GetProductInfo` | `GetSystemMetrics` | `BrandingFormatString` | ntdll.dll   |
| --------------------- | :-------------------: | :-------------: | :----------: | :------------: | :-------------: | :--------------: | :---------------: | :--------------: | :----------------: | :--------------------: | :---------- |
| Win95 A (4.0.950)     |                       |        X        |      X       |       ⁵        |                 |                  |                   |                  |         X          |                        | 4.0.0.950   |
| Win95 OSR2 (4.0.1111) |                       |        X        |      X       |       ⁵        |                 |                  |                   |                  |         X          |                        | 4.0.0.950   |
| Win98 SE (4.10.2222)  |                       |        X        |      X       |       X        |                 |                  |                   |                  |         X          |                        | 4.10.0.2222 |
| Win ME (4.90.3000)    |                       |        X        |      X       |       X        |                 |                  |                   |                  |         X          |                        | 4.90.0.3000 |

| API Support               | `GetNativeSystemInfo` | `GetSystemInfo` | `GetVersion` | `GetVersionEx` | `RtlGetVersion` | `IsWow64Process` | `IsWow64Process2` | `GetProductInfo` | `GetSystemMetrics` | `BrandingFormatString` | ntdll.dll          |
| ------------------------- | :-------------------: | :-------------: | :----------: | :------------: | :-------------: | :--------------: | :---------------: | :--------------: | :----------------: | :--------------------: | :----------------- |
| WinNT 3.51 (3.51.1057.1)  |                       |        X        |      X       |       ⁵        |                 |                  |                   |                  |         X          |                        | 3.51.1025.1        |
| WinNT 4.0 SP1 (4.0.1381)  |                       |        X        |      X       |       ⁵        |                 |                  |                   |                  |         X          |                        | 4.0.1376.1         |
| WinNT 4.0 SP6a (4.0.1381) |                       |        X        |      X       |       X        |                 |                  |                   |                  |         X          |                        | 4.0.1381.298       |
| Win 2000 SP4 (5.0.2195)   |                       |        X        |      X       |       X        |        ⁴        |                  |                   |                  |         X          |                        | 5.0.2195.6899      |
| WinXP (5.1.2600)          |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 5.1.2600.0         |
| WinXP SP3 (5.1.2600)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 5.1.2600.6055      |
| Vista SP1 (6.0.6001)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.0.6001.18000     |
| Vista SP2 (6.0.6002)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.0.6002.18005     |
| Win 7 SP1 (6.1.7601)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.1.7601.24545     |
| Win 8 (6.2.9200)          |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.2.9200.16384     |
| Win 8.1 SP3 (6.3.9600)    |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.3.9600.17415     |
| Win 10 1507 (10.0.10240)  |           X           |        X        |      X       |       ¹        |        X        |        X         |                   |        X         |         X          |           X            | 10.0.10240.17184 ² |
| Win 10 1511 (10.0.10586)  |           X           |        X        |      X       |       ¹        |        X        |        X         |                   |        X         |         X          |           X            | 10.0.10586.0 ²     |
| Win 10 1607 (10.0.14393)  |           X           |        X        |      X       |       ¹        |        X        |        X         |                   |        X         |         X          |           X            | 10.0.14393.447 ²   |
| Win 10 1709 (10.0.16299)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.16299.15 ²    |
| Win 10 1803 (10.0.17134)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.17134.799 ²   |
| Win 10 1809 (10.0.17763)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.17763.1 ²     |
| Win 10 1903 (10.0.18362)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.18362.1 ²     |
| Win 10 21H2 (10.0.19044)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.19041.5794 ²  |
| Win 10 22H2 (10.0.19045)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.19041.5794 ²  |
| Win 11 21H2 (10.0.22000)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           ³            | 10.0.22621.5413 ²  |
| Win 11 23H2 (10.0.22631)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           ³            | 10.0.22621.5413 ²  |
| Win 11 24H2 (10.0.26100)  |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           ³            | 10.0.26100.4061 ²  |

| API Support               | `GetNativeSystemInfo` | `GetSystemInfo` | `GetVersion` | `GetVersionEx` | `RtlGetVersion` | `IsWow64Process` | `IsWow64Process2` | `GetProductInfo` | `GetSystemMetrics` | `BrandingFormatString` | ntdll.dll         |
| ------------------------- | :-------------------: | :-------------: | :----------: | :------------: | :-------------: | :--------------: | :---------------: | :--------------: | :----------------: | :--------------------: | :---------------- |
| Win 2003 SP2 (5.2.3790)   |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 5.2.3790.3959     |
| Win 2003R2 SP1 (5.2.3790) |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 5.2.3790.1830     |
| Win 2008 SP1 (6.0.6001)   |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.0.6001.18000    |
| Win 2008R2 SP1 (6.1.7601) |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.1.7601.17514    |
| SBS 2011 (6.1.7600)       |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.1.7600.16385    |
| SBS 2011 SP1 (6.1.7601)   |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.1.7601.24545    |
| Win 2012 (6.2.9200)       |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.2.9200.16384    |
| Win 2012R2 (6.3.9600)     |           X           |        X        |      X       |       ¹        |        X        |        X         |                   |        X         |         X          |           X            | 6.3.9600.17415    |
| Win 2016 (10.0.14393)     |           X           |        X        |      X       |       ¹        |        X        |        X         |                   |        X         |         X          |           X            | 10.0.14393.0 ²    |
| Win 2019 (10.0.17763)     |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.17763.2686 ² |
| Win 2022 (10.0.20348)     |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.20348.143 ²  |
| Win 2025 (10.0.26100)     |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | 10.0.26100.1882 ² |

| API Support    | `GetNativeSystemInfo` | `GetSystemInfo` | `GetVersion` | `GetVersionEx` | `RtlGetVersion` | `IsWow64Process` | `IsWow64Process2` | `GetProductInfo` | `GetSystemMetrics` | `BrandingFormatString` | ntdll.dll      |
| -------------- | :-------------------: | :-------------: | :----------: | :------------: | :-------------: | :--------------: | :---------------: | :--------------: | :----------------: | :--------------------: | :------------- |
| ReactOS 0.4.15 |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 42.4.15.0      |
| Wine 6.0.3     |           X           |        X        |      X       |       X        |        X        |        X         |         X         |        X         |         X          |                        | 6.1.7601.24059 |

- ¹ `GetVersionEx` reports `6.2.9200`, instead of the correct version, when no
  manifest file declaring compatibility is present.
- ² The file version build number doesn't match the `GetVersionEx` build number
  - On Windows 10.19045, it still returns 19041. So the file of `ntdll.dll`
    can't be used to determine the Windows version reliably.
  - On Windows 11.22631, it still returns 22621.
  - When missing the manifest file. E.g. on Windows 10.0.19045 with no manifest,
    the file version would be `6.2.19041`.
- ³ The short branding string on Windows 11, still shows Windows 10.
- ⁴ Present, but CSD string is empty.
- ⁵ The `GetVersionEx` only supports the `OSVERSIONINFO` structure, and provides
  no Product information.
