#include "stdafx.h"

#include <malloc.h>

#include "win32_api.h"

typedef HANDLE(WINAPI *fGetCurrentProcess)();
typedef void (WINAPI *fGetNativeSystemInfo)(LPSYSTEM_INFO);
typedef void (WINAPI *fGetSystemInfo)(LPSYSTEM_INFO);
typedef DWORD(WINAPI *fGetVersion)();
typedef BOOL(WINAPI *fIsWow64Process)(HANDLE hProcess, PBOOL Wow64Process);
typedef BOOL(WINAPI *fIsWow64Process2)(HANDLE hProcess, USHORT *pProcessMachine, USHORT *pNativeMachine);
typedef BOOL(WINAPI *fGetProductInfo)(DWORD dwOSMajorVersion, DWORD dwOSMinorVersion, DWORD dwSpMajorVersion, DWORD dwSpMinorVersion, PDWORD pdwReturnedProductType);
typedef int (WINAPI *fGetSystemMetrics)(int nIndex);
typedef BOOL(WINAPI *fGetVersionEx)(LPOSVERSIONINFO);
typedef NTSTATUS(WINAPI *fRtlGetVersion)(LPOSVERSIONINFOEXW);
typedef LPTSTR(WINAPI *fBrandingFormatString)(LPCTSTR format);

struct win32
{
    HMODULE lib_kernel32;
    HMODULE lib_user32;
    HMODULE lib_ntdll;
    HMODULE lib_winbrand;

    fGetCurrentProcess GetCurrentProcess;
    fGetNativeSystemInfo GetNativeSystemInfo;
    fGetSystemInfo GetSystemInfo;
    fGetVersion GetVersion;
    fGetVersionEx GetVersionEx;
    fIsWow64Process IsWow64Process;
    fIsWow64Process2 IsWow64Process2;
    fGetProductInfo GetProductInfo;
    fGetSystemMetrics GetSystemMetrics;

    fRtlGetVersion RtlGetVersion;

    fBrandingFormatString BrandingFormatString;
};

win32_t *init_win32_lib()
{
    win32_t *x = (win32_t *)malloc(sizeof(win32_t));
    if (x == NULL) return NULL;
    memset(x, 0, sizeof(win32_t));

    x->lib_kernel32 = LoadLibraryA("kernel32.dll");
    x->lib_user32 = LoadLibraryA("user32.dll");
    x->lib_ntdll = LoadLibraryA("ntdll.dll");
    x->lib_winbrand = LoadLibraryA("winbrand.dll");

    if (x->lib_kernel32) {
        x->GetCurrentProcess = (fGetCurrentProcess)GetProcAddress(x->lib_kernel32, "GetCurrentProcess");
        x->GetNativeSystemInfo = (fGetNativeSystemInfo)GetProcAddress(x->lib_kernel32, "GetNativeSystemInfo");
        x->GetSystemInfo = (fGetSystemInfo)GetProcAddress(x->lib_kernel32, "GetSystemInfo");
        x->GetVersion = (fGetVersion)GetProcAddress(x->lib_kernel32, "GetVersion");
        x->IsWow64Process = (fIsWow64Process)GetProcAddress(x->lib_kernel32, "IsWow64Process");
        x->IsWow64Process2 = (fIsWow64Process2)GetProcAddress(x->lib_kernel32, "IsWow64Process2");
        x->GetProductInfo = (fGetProductInfo)GetProcAddress(x->lib_kernel32, "GetProductInfo");
#ifdef _UNICODE
        x->GetVersionEx = (fGetVersionEx)GetProcAddress(x->lib_kernel32, "GetVersionExW");
#else
        x->GetVersionEx = (fGetVersionEx)GetProcAddress(x->lib_kernel32, "GetVersionExA");
#endif
    }

    if (x->lib_user32) {
        x->GetSystemMetrics = (fGetSystemMetrics)GetProcAddress(x->lib_user32, "GetSystemMetrics");
    }

    if (x->lib_ntdll) {
        x->RtlGetVersion = (fRtlGetVersion)GetProcAddress(x->lib_ntdll, "RtlGetVersion");
    }

    if (x->lib_winbrand) {
#ifdef _UNICODE
        x->BrandingFormatString = (fBrandingFormatString)GetProcAddress(x->lib_winbrand, "BrandingFormatString");
#endif
    }

    return x;
}

void term_win32_lib(win32_t *handle)
{
    if (handle->lib_kernel32 != NULL) FreeLibrary(handle->lib_kernel32);
    if (handle->lib_ntdll != NULL) FreeLibrary(handle->lib_ntdll);
    if (handle->lib_winbrand != NULL) FreeLibrary(handle->lib_winbrand);
    free(handle);
}

HMODULE win32_ntdll(win32_t *handle)
{
    if (handle == NULL) return NULL;
    return handle->lib_ntdll;
}

HMODULE win32_kernel32(win32_t *handle)
{
    if (handle == NULL) return NULL;
    return handle->lib_kernel32;
}

BOOL win32_GetCurrentProcess(win32_t *handle, HANDLE *result)
{
    HANDLE res;
    if (!handle || !handle->GetCurrentProcess) return FALSE;

    res = handle->GetCurrentProcess();
    if (result) *result = res;
    return TRUE;
}

BOOL win32_GetNativeSystemInfo(win32_t *handle, LPSYSTEM_INFO lpSystemInfo)
{
    if (!handle || !handle->GetNativeSystemInfo) return FALSE;

    handle->GetNativeSystemInfo(lpSystemInfo);
    return TRUE;
}

BOOL win32_GetSystemInfo(win32_t *handle, LPSYSTEM_INFO lpSystemInfo)
{
    if (!handle || !handle->GetSystemInfo) return FALSE;

    handle->GetSystemInfo(lpSystemInfo);
    return TRUE;
}

BOOL win32_GetVersion(win32_t *handle, DWORD *result)
{
    DWORD res;
    if (!handle || !handle->GetVersion) return FALSE;

    res = handle->GetVersion();
    if (result) *result = res;
    return TRUE;
}

BOOL win32_GetVersionEx(win32_t *handle, BOOL *result, LPOSVERSIONINFO lpVersionInformation)
{
    BOOL res;
    if (!handle || !handle->GetVersionEx) return FALSE;

    res = handle->GetVersionEx(lpVersionInformation);
    if (result) *result = res;
    return TRUE;
}

BOOL win32_RtlGetVersion(win32_t *handle, NTSTATUS *result, LPOSVERSIONINFOEX lpVersionInformation)
{
    NTSTATUS res;
    if (!handle || !handle->RtlGetVersion) return FALSE;

    res = handle->RtlGetVersion(lpVersionInformation);
    if (result) *result = res;
    return TRUE;
}

BOOL win32_IsWow64Process(win32_t *handle, BOOL *result, HANDLE hProcess, PBOOL Wow64Process)
{
    BOOL res;
    if (!handle || !handle->IsWow64Process) return FALSE;

    res = handle->IsWow64Process(hProcess, Wow64Process);
    if (result) *result = res;
    return TRUE;
}

BOOL win32_IsWow64Process2(win32_t *handle, BOOL *result, HANDLE hProcess, USHORT *pProcessMachine, USHORT *pNativeMachine)
{
    BOOL res;
    if (!handle || !handle->IsWow64Process2) return FALSE;

    res = handle->IsWow64Process2(hProcess, pProcessMachine, pNativeMachine);
    if (result) *result = res;
    return TRUE;
}

BOOL win32_GetProductInfo(win32_t *handle, BOOL *result, DWORD dwOSMajorVersion, DWORD dwOSMinorVersion, DWORD dwSpMajorVersion, DWORD dwSpMinorVersion, PDWORD pdwReturnedProductType)
{
    BOOL res;
    if (!handle || !handle->GetProductInfo) return FALSE;

    res = handle->GetProductInfo(dwOSMajorVersion, dwOSMinorVersion, dwSpMajorVersion, dwSpMinorVersion, pdwReturnedProductType);
    if (result) *result = res;
    return TRUE;
}

BOOL win32_GetSystemMetrics(win32_t *handle, int *result, int nIndex)
{
    int res;
    if (!handle || !handle->GetSystemMetrics) return FALSE;

    res = handle->GetSystemMetrics(nIndex);
    if (result) *result = res;
    return TRUE;
}

BOOL win32_BrandingFormatString(win32_t *handle, LPTSTR *result, LPCTSTR format)
{
    LPTSTR res;
    if (!handle || !handle->BrandingFormatString) return FALSE;

    res = handle->BrandingFormatString(format);
    if (result) *result = res;
    return TRUE;
}
