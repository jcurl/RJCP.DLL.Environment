#ifndef WIN32_API_H
#define WIN32_API_H

#include "stdafx.h"

struct win32;
typedef struct win32 win32_t;

/// <summary>
/// Initialises the libraries.
/// </summary>
/// <returns>A handle to be used when calling Win32 API.</returns>
win32_t *init_win32_lib();

/// <summary>
/// Frees the libraries.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
void term_win32_lib(win32_t *handle);

/// <summary>
/// Get the hModule for ntdll.dll.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <returns>The hModule for accessing the ntdll.dll library.</returns>
HMODULE win32_ntdll(win32_t *handle);

/// <summary>
/// Get the hModule for kernel32.dll.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <returns>The hModule for accessing the kernel32.dll library.</returns>
HMODULE win32_kernel32(win32_t *handle);

/// <summary>
/// GetCurrentProcess.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of GetCurrentProcess.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetCurrentProcess(win32_t *handle, HANDLE *result);

/// <summary>
/// GetNativeSystemInfo.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="lpSystemInfo">Structure to receive the information.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetNativeSystemInfo(win32_t *handle, LPSYSTEM_INFO lpSystemInfo);

/// <summary>
/// GetSystemInfo.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="lpSystemInfo">Structure to receive the information.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetSystemInfo(win32_t *handle, LPSYSTEM_INFO lpSystemInfo);

/// <summary>
/// GetVersion.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of GetVersion.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetVersion(win32_t *handle, DWORD *result);

/// <summary>
/// GetVersionEx.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of GetVersionExA.</param>
/// <param name="lpVersionInformation">Receives the result</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetVersionEx(win32_t *handle, BOOL *result, LPOSVERSIONINFO lpVersionInformation);

/// <summary>
/// RtlGetVersion.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of RtlGetVersion.</param>
/// <param name="lpVersionInformation">Receives the result</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_RtlGetVersion(win32_t *handle, NTSTATUS *result, LPOSVERSIONINFOEXW lpVersionInformation);

/// <summary>
/// IsWow64Process.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of IsWow64Process.</param>
/// <param name="hProcess">A handle to the process.</param>
/// <param name="Wow64Process">A pointer to a value that is TRUE if the process is running under WOW64</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_IsWow64Process(win32_t *handle, BOOL *result, HANDLE hProcess, PBOOL Wow64Process);

/// <summary>
/// IsWow64Process2.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of IsWow64Process2.</param>
/// <param name="hProcess">A handle to the process.</param>
/// <param name="pProcessMachine">Receives a pointer to a IMAGE_FILE_MACHINE_*. Is IMAGE_FILE_MACHINE_UNKNOWN if this is not a WOW64 process.</param>
/// <param name="pNativeMachine">Receives a pointer to IMAGE_FILE_MACHINE_* for that native architecture.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_IsWow64Process2(win32_t *handle, BOOL *result, HANDLE hProcess, USHORT *pProcessMachine, USHORT *pNativeMachine);

/// <summary>
/// GetProductInfo.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of GetProductInfo.</param>
/// <param name="dwOSMajorVersion">Major version. Minimum value is 6.</param>
/// <param name="dwOSMinorVersion">Minor version. Minimum value is 0.</param>
/// <param name="dwSpMajorVersion">Major service pack version. Minimum value is 0.</param>
/// <param name="dwSpMinorVersion">Minor service pack version. Minimum value is 0.</param>
/// <param name="pdwReturnedProductType">Receives the product type code.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetProductInfo(win32_t *handle, BOOL *result, DWORD dwOSMajorVersion, DWORD dwOSMinorVersion, DWORD dwSpMajorVersion, DWORD dwSpMinorVersion, PDWORD pdwReturnedProductType);

/// <summary>
/// GetSystemMetrics.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of GetSystemMetrics.</param>
/// <param name="nIndex">The metric index to obtain.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_GetSystemMetrics(win32_t *handle, int *result, int nIndex);

/// <summary>
/// BrandingFormatString.
/// </summary>
/// <param name="handle">The handle returned from initialise_libraries().</param>
/// <param name="result">The result of BrandingFormatString.</param>
/// <param name="format">The formatting string.</param>
/// <returns>On success returns 0, returns 1 if function is unavailable.</returns>
BOOL win32_BrandingFormatString(win32_t *handle, LPTSTR *result, LPCTSTR format);

#endif
