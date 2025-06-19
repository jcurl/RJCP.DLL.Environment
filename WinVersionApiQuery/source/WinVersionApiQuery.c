#include "stdafx.h"
#include <tchar.h>
#include <stdio.h>

#include "win32_api.h"
#include "win32_dump.h"

#define STATUS_SUCCESS 0

static void native_system_info(win32_t *handle, writehandle_t *whandle)
{
    SYSTEM_INFO native_system_info;

    if (win32_GetNativeSystemInfo(handle, &native_system_info)) {
        print_system_info(whandle, TEXT("GetNativeSystemInfo"), &native_system_info);
    }
}

static void system_info(win32_t *handle, writehandle_t *whandle)
{
    SYSTEM_INFO system_info;

    if (win32_GetSystemInfo(handle, &system_info)) {
        print_system_info(whandle, TEXT("GetSystemInfo"), &system_info);
    }
}

static DWORD win_version(win32_t *handle, writehandle_t *whandle)
{
    DWORD win_version;

    if (win32_GetVersion(handle, &win_version)) {
        print_version(whandle, win_version);
    }
    return win_version;
}

static BOOL win_version_osver(win32_t *handle, writehandle_t *whandle)
{
    OSVERSIONINFO version_information = { 0, };
    BOOL version_information_result;

    version_information.dwOSVersionInfoSize = sizeof(version_information);
    if (win32_GetVersionEx(handle, &version_information_result, &version_information) && version_information_result) {
        print_version_info(whandle, version_information_result, &version_information);
    }

    return version_information_result;
}

static BOOL win_version_osverex(win32_t *handle, writehandle_t *whandle, LPOSVERSIONINFOEX ver)
{
    BOOL version_information_ex_result;

    memset(ver, 0, sizeof(OSVERSIONINFOEX));
    ver->dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);
    if (win32_GetVersionEx(handle, &version_information_ex_result, (LPOSVERSIONINFO)ver) && version_information_ex_result) {
        print_version_info_ex(whandle, version_information_ex_result, ver);
    }
    return version_information_ex_result;
}

static BOOL win_version_rtl(win32_t *handle, writehandle_t *whandle, LPOSVERSIONINFOEXW ver)
{
    NTSTATUS rtl_version_information_result;

    memset(ver, 0, sizeof(OSVERSIONINFOEXW));
    ver->dwOSVersionInfoSize = sizeof(OSVERSIONINFOEXW);
    if (win32_RtlGetVersion(handle, &rtl_version_information_result, ver) && rtl_version_information_result == STATUS_SUCCESS) {
        print_version_info_nt(whandle, rtl_version_information_result, ver);
    }

    return rtl_version_information_result == STATUS_SUCCESS;
}

static void is_wow64_process(win32_t *handle, writehandle_t *whandle)
{
    HANDLE current_process = NULL;

    if (win32_GetCurrentProcess(handle, &current_process)) {
        BOOL wow_result;
        BOOL is_wow_process;

        if (win32_IsWow64Process(handle, &wow_result, current_process, &is_wow_process) && wow_result) {
            print_is_wow64_process(whandle, wow_result, is_wow_process);
        }
    }
}

static void is_wow64_process2(win32_t *handle, writehandle_t *whandle)
{
    HANDLE current_process = NULL;

    if (win32_GetCurrentProcess(handle, &current_process)) {
        BOOL wow2_result;
        USHORT process_machine;
        USHORT native_machine;

        if (win32_IsWow64Process2(handle, &wow2_result, current_process, &process_machine, &native_machine) && wow2_result) {
            print_is_wow64_process2(whandle, wow2_result, process_machine, native_machine);
        }
    }
}

static void product_info(win32_t *handle, writehandle_t *whandle, LPOSVERSIONINFOEX ver)
{
    BOOL product_info_result;
    DWORD product_type;

    if (win32_GetProductInfo(handle, &product_info_result, ver->dwMajorVersion, ver->dwMinorVersion, ver->wServicePackMajor, ver->wServicePackMinor, &product_type)) {
        print_product_info(whandle, product_info_result, ver->dwMajorVersion, ver->dwMinorVersion, ver->wServicePackMajor, ver->wServicePackMinor, product_type);
    }
}

static void product_infow(win32_t *handle, writehandle_t *whandle, LPOSVERSIONINFOEXW ver)
{
    BOOL product_info_result;
    DWORD product_type;

    if (win32_GetProductInfo(handle, &product_info_result, ver->dwMajorVersion, ver->dwMinorVersion, ver->wServicePackMajor, ver->wServicePackMinor, &product_type)) {
        print_product_info(whandle, product_info_result, ver->dwMajorVersion, ver->dwMinorVersion, ver->wServicePackMajor, ver->wServicePackMinor, product_type);
    }
}

static void system_metrics(win32_t *handle, writehandle_t *whandle, int metric)
{
    int metric_result;

    if (win32_GetSystemMetrics(handle, &metric_result, metric)) {
        print_system_metric(whandle, metric_result, metric);
    }
}

static void brand(win32_t *handle, writehandle_t *whandle, LPTSTR format)
{
    LPTSTR brand = NULL;

    if (win32_BrandingFormatString(handle, &brand, format)) {
        print_branding_string(whandle, brand, format);
        GlobalFree(brand);
    }
}

int _tmain(int argc, TCHAR **argv)
{
    DWORD winversion;
    BOOL version_information_ex_result;
    OSVERSIONINFOEX version_information_ex;
    BOOL rtl_version_information_result;
    OSVERSIONINFOEXW rtl_version_information;

    win32_t *handle;
    writehandle_t *whandle;

    handle = init_win32_lib();
    if (handle == NULL) {
        fprintf(stderr, "Couldn't initialise libraries");
        return 1;
    }

    whandle = init_write(argc == 2 ? argv[1] : NULL);

    print_header(whandle);

    native_system_info(handle, whandle);
    system_info(handle, whandle);
    winversion = win_version(handle, whandle);
    win_version_osver(handle, whandle);
    version_information_ex_result = win_version_osverex(handle, whandle, &version_information_ex);
    rtl_version_information_result = win_version_rtl(handle, whandle, &rtl_version_information);

    is_wow64_process(handle, whandle);
    is_wow64_process2(handle, whandle);

    if (version_information_ex_result) product_info(handle, whandle, &version_information_ex);
    if (rtl_version_information_result) product_infow(handle, whandle, &rtl_version_information);

    system_metrics(handle, whandle, SM_DEBUG);
    system_metrics(handle, whandle, SM_MEDIACENTER);
    system_metrics(handle, whandle, SM_TABLETPC);
    system_metrics(handle, whandle, SM_STARTER);
    system_metrics(handle, whandle, SM_SERVERR2);

    brand(handle, whandle, TEXT("%WINDOWS_SHORT%"));
    brand(handle, whandle, TEXT("%WINDOWS_LONG%"));
    brand(handle, whandle, TEXT("%WINDOWS_GENERIC%"));
    brand(handle, whandle, TEXT("%WINDOWS_PRODUCT%"));
    brand(handle, whandle, TEXT("%WINDOWS_COPYRIGHT%"));
    brand(handle, whandle, TEXT("%MICROSOFT_COMPANYNAME%"));

    dump_registry_key(whandle, HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"));
    dump_registry_key(whandle, HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows\\CurrentVersion"));

    dump_library_version(whandle, win32_kernel32(handle));
    dump_library_version(whandle, win32_ntdll(handle));

    print_trailer(whandle);

    term_write(whandle);
    term_win32_lib(handle);
    return 0;
}
