#include <Windows.h>
#include <tchar.h>
#include <stdio.h>

#include "win32_api.h"
#include "win32_dump.h"

#define STATUS_SUCCESS 0

int _tmain(int argc, TCHAR **argv)
{
    win32_t *handle = init_win32_lib();
    if (handle == NULL) {
        fprintf(stderr, "Couldn't initialise libraries");
        return 1;
    }

    writehandle_t *whandle = init_write(argc == 2 ? argv[1] : NULL);

    print_header(whandle);

    SYSTEM_INFO native_system_info;
    if (win32_GetNativeSystemInfo(handle, &native_system_info)) {
        print_system_info(whandle, TEXT("GetNativeSystemInfo"), &native_system_info);
    }

    SYSTEM_INFO system_info;
    if (win32_GetSystemInfo(handle, &system_info)) {
        print_system_info(whandle, TEXT("GetSystemInfo"), &system_info);
    }

    DWORD win_version;
    if (win32_GetVersion(handle, &win_version)) {
        print_version(whandle, win_version);
    }

    OSVERSIONINFO version_information = { 0, };
    version_information.dwOSVersionInfoSize = sizeof(version_information);
    BOOL version_information_result;
    if (win32_GetVersionEx(handle, &version_information_result, &version_information) && version_information_result) {
        print_version_info(whandle, version_information_result, &version_information);
    }

    OSVERSIONINFOEX version_information_ex = { 0, };
    version_information_ex.dwOSVersionInfoSize = sizeof(version_information_ex);
    BOOL version_information_ex_result;
    if (win32_GetVersionEx(handle, &version_information_ex_result, (LPOSVERSIONINFO)(&version_information_ex)) && version_information_ex_result) {
        print_version_info_ex(whandle, version_information_ex_result, &version_information_ex);
    }

    OSVERSIONINFOEX rtl_version_information = { 0, };
    rtl_version_information.dwOSVersionInfoSize = sizeof(rtl_version_information);
    NTSTATUS rtl_version_information_result;
    if (win32_RtlGetVersion(handle, &rtl_version_information_result, &rtl_version_information) && rtl_version_information_result == STATUS_SUCCESS) {
        print_version_info_nt(whandle, rtl_version_information_result, &rtl_version_information);
    }

    HANDLE current_process = NULL;
    if (win32_GetCurrentProcess(handle, &current_process)) {
        BOOL wow_result;
        BOOL is_wow_process;
        if (win32_IsWow64Process(handle, &wow_result, current_process, &is_wow_process) && wow_result) {
            print_is_wow64_process(whandle, wow_result, is_wow_process);
        }

        BOOL wow2_result;
        USHORT process_machine;
        USHORT native_machine;
        if (win32_IsWow64Process2(handle, &wow2_result, current_process, &process_machine, &native_machine) && wow2_result) {
            print_is_wow64_process2(whandle, wow2_result, process_machine, native_machine);
        }
    }

    if (version_information_ex_result) {
        BOOL product_info_result;
        DWORD product_type;
        if (win32_GetProductInfo(handle, &product_info_result, version_information_ex.dwMajorVersion, version_information_ex.dwMinorVersion, version_information_ex.wServicePackMajor, version_information_ex.wServicePackMinor, &product_type)) {
            print_product_info(whandle, product_info_result, version_information_ex.dwMajorVersion, version_information_ex.dwMinorVersion, version_information_ex.wServicePackMajor, version_information_ex.wServicePackMinor, product_type);
        }
    }

    if (rtl_version_information_result == STATUS_SUCCESS) {
        BOOL product_info_result;
        DWORD product_type;
        if (win32_GetProductInfo(handle, &product_info_result, rtl_version_information.dwMajorVersion, rtl_version_information.dwMinorVersion, rtl_version_information.wServicePackMajor, rtl_version_information.wServicePackMinor, &product_type)) {
            print_product_info(whandle, product_info_result, rtl_version_information.dwMajorVersion, rtl_version_information.dwMinorVersion, rtl_version_information.wServicePackMajor, rtl_version_information.wServicePackMinor, product_type);
        }
    }

    int metrics_debug;
    if (win32_GetSystemMetrics(handle, &metrics_debug, SM_DEBUG)) {
        print_system_metric(whandle, metrics_debug, SM_DEBUG);
    }

    int metrics_mediacenter;
    if (win32_GetSystemMetrics(handle, &metrics_mediacenter, SM_MEDIACENTER)) {
        print_system_metric(whandle, metrics_mediacenter, SM_MEDIACENTER);
    }

    int metrics_tabletpc;
    if (win32_GetSystemMetrics(handle, &metrics_tabletpc, SM_TABLETPC)) {
        print_system_metric(whandle, metrics_tabletpc, SM_TABLETPC);
    }

    int metrics_starter;
    if (win32_GetSystemMetrics(handle, &metrics_starter, SM_STARTER)) {
        print_system_metric(whandle, metrics_starter, SM_STARTER);
    }

    int metrics_serverr2;
    if (win32_GetSystemMetrics(handle, &metrics_serverr2, SM_SERVERR2)) {
        print_system_metric(whandle, metrics_serverr2, SM_SERVERR2);
    }

    LPTSTR brand = NULL;
    if (win32_BrandingFormatString(handle, &brand, TEXT("%WINDOWS_SHORT%"))) {
        print_branding_string(whandle, brand, TEXT("%WINDOWS_SHORT%"));
        GlobalFree(brand);
    }

    if (win32_BrandingFormatString(handle, &brand, TEXT("%WINDOWS_LONG%"))) {
        print_branding_string(whandle, brand, TEXT("%WINDOWS_LONG%"));
        GlobalFree(brand);
    }

    if (win32_BrandingFormatString(handle, &brand, TEXT("%WINDOWS_GENERIC%"))) {
        print_branding_string(whandle, brand, TEXT("%WINDOWS_GENERIC%"));
        GlobalFree(brand);
    }

    if (win32_BrandingFormatString(handle, &brand, TEXT("%WINDOWS_PRODUCT%"))) {
        print_branding_string(whandle, brand, TEXT("%WINDOWS_PRODUCT%"));
        GlobalFree(brand);
    }

    if (win32_BrandingFormatString(handle, &brand, TEXT("%WINDOWS_COPYRIGHT%"))) {
        print_branding_string(whandle, brand, TEXT("%WINDOWS_COPYRIGHT%"));
        GlobalFree(brand);
    }

    if (win32_BrandingFormatString(handle, &brand, TEXT("%MICROSOFT_COMPANYNAME%"))) {
        print_branding_string(whandle, brand, TEXT("%MICROSOFT_COMPANYNAME%"));
        GlobalFree(brand);
    }

    dump_registry_key(whandle, HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"));

    dump_library_version(whandle, win32_kernel32(handle));

    dump_library_version(whandle, win32_ntdll(handle));

    print_trailer(whandle);

    term_write(whandle);
    term_win32_lib(handle);
    return 0;
}
