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

| API                       | `GetNativeSystemInfo` | `GetSystemInfo` | `GetVersion` | `GetVersionEx` | `RtlGetVersion` | `IsWow64Process` | `IsWow64Process2` | `GetProductInfo` | `GetSystemMetrics` | `BrandingFormatString` | ntdll.dll      |
| ------------------------- | :-------------------: | :-------------: | :----------: | :------------: | :-------------: | :--------------: | :---------------: | :--------------: | :----------------: | :--------------------: | :------------- |
| WinNT 4.0 SP1 (4.0.1381)  |                       |        X        |      X       |       ⁵        |                 |                  |                   |                  |                    |                        | 4.0.1376.1     |
| WinNT 4.0 SP6a (4.0.1381) |                       |        X        |      X       |       X        |                 |                  |                   |                  |         X          |                        | 4.0.1381.298   |
| Win 2000 SP4 (5.0.2195)   |                       |        X        |      X       |       X        |        ⁴        |                  |                   |                  |         X          |                        | 5.0.2195.6899  |
| WinXP SP3 (5.1.2600)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 5.1.2600.6055  |
| Win 2003 (5.2.3790)       |           X           |        X        |      X       |       X        |        X        |        X         |                   |                  |         X          |                        | 5.2.3790.3959  |
| Vista SP1 (6.0.6001)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.0.6001.18000 |
| Vista SP2 (6.0.6002)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.0.6002.18005 |
| Win 7 SP1 (6.1.7601)      |           X           |        X        |      X       |       X        |        X        |        X         |                   |        X         |         X          |           X            | 6.1.7601.24545 |
| Win 10.17134 (10.0.17134) |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | ² 6.2.17134    |
| Win 10.19045 (10.0.19045) |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           X            | ² 6.2.19041    |
| Win 11.22631 (10.0.22631) |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           ³            | ² 6.2.22621    |
| Win 11.26100 (10.0.26100) |           X           |        X        |      X       |       ¹        |        X        |        X         |         X         |        X         |         X          |           ³            | ² 6.2.26100    |

- ¹ `GetVersion*` reports `6.2.9200`; file version also reports `6.2.<build>`,
  when binary has no manifest file.
- ² The file version build number doesn't match the `GetVersionEx` build number
  - On Windows 10.19045, it still returns 19041. So the file of `ntdll.dll`
    can't be used to determine the Windows version reliably.
  - On Windows 11.22631, it still returns 22621.
  - When missing the manifest file. E.g. on Windows 10.0.19045 with no manifest,
    the file version would be `6.2.19041`.
- ³ The short branding string on Windows 11, still shows Windows 10.
- ⁴ Present, but CSD string is empty.
- ⁵ The `GetVersionEx` only supports the `OSVERSIONINFO` structure, and provides
  no Service Pack information.