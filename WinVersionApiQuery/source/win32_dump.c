#define _CRT_SECURE_NO_DEPRECATE
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"
#include <tchar.h>
#include <stdio.h>

#include "win32_dump.h"

// Data based on the SDK 10.0.26100

struct writehandle
{
    BOOL close_file;
    FILE *out_file;
};

writehandle_t *init_write(LPCTSTR file_name)
{
    writehandle_t *w = (writehandle_t *)malloc(sizeof(writehandle_t));
    if (w == NULL) return NULL;
    memset(w, 0, sizeof(writehandle_t));

    if (file_name == NULL) {
        w->close_file = FALSE;
        w->out_file = stdout;
    } else {
        w->close_file = TRUE;
#ifdef _UNICODE
        w->out_file = _tfopen(file_name, TEXT("w, ccs=UNICODE"));
#else
        w->out_file = _tfopen(file_name, TEXT("w"));
#endif
        if (w->out_file == NULL) return NULL;
    }
    return w;
}

void term_write(writehandle_t *handle)
{
    if (handle && handle->close_file && handle->out_file) {
        fclose(handle->out_file);
        handle->out_file = NULL;
    }
}

void print_header(writehandle_t *handle)
{
    if (!handle) return;

#ifdef _UNICODE
    _ftprintf(handle->out_file, TEXT("<?xml version=\"1.0\" encoding=\"UTF-16\"?>\n"));
#else
    _ftprintf(handle->out_file, TEXT("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"));
#endif
    _ftprintf(handle->out_file, TEXT("<WinVersionQuery>\n"));
    _ftprintf(handle->out_file, TEXT("  <API>\n"));
}

void print_trailer(writehandle_t *handle)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("  </API>\n"));
    _ftprintf(handle->out_file, TEXT("</WinVersionQuery>\n"));
}

static void print_field_byte(writehandle_t *handle, TCHAR *name, BYTE value)
{
    _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%u</Field>\n"), name, value);
}

static void print_field_word(writehandle_t *handle, TCHAR *name, WORD value)
{
    _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%u</Field>\n"), name, value);
}

static void print_field_dword(writehandle_t *handle, TCHAR *name, DWORD value)
{
    _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%lu</Field>\n"), name, value);
}

static void print_field_void_ptr(writehandle_t *handle, TCHAR *name, LPVOID value)
{
    _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%p</Field>\n"), name, value);
}

static void print_field_dword_ptr(writehandle_t *handle, TCHAR *name, DWORD_PTR value)
{
#pragma warning(push)
#pragma warning(disable: 6328)
#pragma warning(disable: 4477)
    if (sizeof(value) == 8) {
        _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%llu</Field>\n"), name, value);
    } else {
        _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%lu</Field>\n"), name, value);
    }
#pragma warning(pop)
}

static void print_field_tchar(writehandle_t *handle, TCHAR *name, TCHAR *value)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("      <Field name=\"%s\">%s</Field>\n"), name, value);
}

void print_system_info(writehandle_t *handle, TCHAR *api, LPSYSTEM_INFO lpSystemInfo)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <%s>\n"), api);
    print_field_dword(handle, TEXT("dwOemId"), lpSystemInfo->dwOemId);
    print_field_word(handle, TEXT("wProcessorArchitecture"), lpSystemInfo->wProcessorArchitecture);
    print_field_dword(handle, TEXT("dwPageSize"), lpSystemInfo->dwPageSize);
    print_field_void_ptr(handle, TEXT("lpMinimumApplicationAddress"), lpSystemInfo->lpMinimumApplicationAddress);
    print_field_void_ptr(handle, TEXT("lpMaximumApplicationAddress"), lpSystemInfo->lpMaximumApplicationAddress);
    print_field_dword_ptr(handle, TEXT("dwActiveProcessorMask"), lpSystemInfo->dwActiveProcessorMask);
    print_field_dword(handle, TEXT("dwNumberOfProcessors"), lpSystemInfo->dwNumberOfProcessors);
    print_field_dword(handle, TEXT("dwProcessorType"), lpSystemInfo->dwProcessorType);
    print_field_dword(handle, TEXT("dwAllocationGranularity"), lpSystemInfo->dwAllocationGranularity);
    print_field_word(handle, TEXT("wProcessorLevel"), lpSystemInfo->wProcessorLevel);
    print_field_word(handle, TEXT("wProcessorRevision"), lpSystemInfo->wProcessorRevision);
    _ftprintf(handle->out_file, TEXT("    </%s>\n"), api);
}

void print_version(writehandle_t *handle, DWORD value)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <GetVersion return=\"%lu\" />\n"), value);
}

void print_version_info(writehandle_t *handle, BOOL result, LPOSVERSIONINFO lpVersionInformation)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <GetVersionEx result=\"%d\">\n"), result);
    print_field_dword(handle, TEXT("dwOSVersionInfoSize"), lpVersionInformation->dwOSVersionInfoSize);
    print_field_dword(handle, TEXT("dwMajorVersion"), lpVersionInformation->dwMajorVersion);
    print_field_dword(handle, TEXT("dwMinorVersion"), lpVersionInformation->dwMinorVersion);
    print_field_dword(handle, TEXT("dwBuildNumber"), lpVersionInformation->dwBuildNumber);
    print_field_dword(handle, TEXT("dwPlatformId"), lpVersionInformation->dwPlatformId);
    print_field_tchar(handle, TEXT("szCSDVersion"), lpVersionInformation->szCSDVersion);
    _ftprintf(handle->out_file, TEXT("    </GetVersionEx>\n"));
}

void print_version_info_ex(writehandle_t *handle, BOOL result, LPOSVERSIONINFOEX lpVersionInformation)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <GetVersionEx result=\"%d\">\n"), result);
    print_field_dword(handle, TEXT("dwOSVersionInfoSize"), lpVersionInformation->dwOSVersionInfoSize);
    print_field_dword(handle, TEXT("dwMajorVersion"), lpVersionInformation->dwMajorVersion);
    print_field_dword(handle, TEXT("dwMinorVersion"), lpVersionInformation->dwMinorVersion);
    print_field_dword(handle, TEXT("dwBuildNumber"), lpVersionInformation->dwBuildNumber);
    print_field_dword(handle, TEXT("dwPlatformId"), lpVersionInformation->dwPlatformId);
    print_field_tchar(handle, TEXT("szCSDVersion"), lpVersionInformation->szCSDVersion);
    print_field_word(handle, TEXT("wServicePackMajor"), lpVersionInformation->wServicePackMajor);
    print_field_word(handle, TEXT("wServicePackMinor"), lpVersionInformation->wServicePackMinor);
    print_field_word(handle, TEXT("wSuiteMask"), lpVersionInformation->wSuiteMask);
    print_field_byte(handle, TEXT("wProductType"), lpVersionInformation->wProductType);
    _ftprintf(handle->out_file, TEXT("    </GetVersionEx>\n"));
}

void print_version_info_nt(writehandle_t *handle, NTSTATUS result, LPOSVERSIONINFOEX lpVersionInformation)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <RtlGetVersion result=\"%d\">\n"), result);
    print_field_dword(handle, TEXT("dwOSVersionInfoSize"), lpVersionInformation->dwOSVersionInfoSize);
    print_field_dword(handle, TEXT("dwMajorVersion"), lpVersionInformation->dwMajorVersion);
    print_field_dword(handle, TEXT("dwMinorVersion"), lpVersionInformation->dwMinorVersion);
    print_field_dword(handle, TEXT("dwBuildNumber"), lpVersionInformation->dwBuildNumber);
    print_field_dword(handle, TEXT("dwPlatformId"), lpVersionInformation->dwPlatformId);
    print_field_tchar(handle, TEXT("szCSDVersion"), lpVersionInformation->szCSDVersion);
    print_field_word(handle, TEXT("wServicePackMajor"), lpVersionInformation->wServicePackMajor);
    print_field_word(handle, TEXT("wServicePackMinor"), lpVersionInformation->wServicePackMinor);
    print_field_word(handle, TEXT("wSuiteMask"), lpVersionInformation->wSuiteMask);
    print_field_byte(handle, TEXT("wProductType"), lpVersionInformation->wProductType);
    _ftprintf(handle->out_file, TEXT("    </RtlGetVersion>\n"));
}

void print_is_wow64_process(writehandle_t *handle, BOOL result, BOOL value)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <IsWow64Process result=\"%d\" Wow64Process=\"%d\" />\n"), result, value);
}

void print_is_wow64_process2(writehandle_t *handle, BOOL result, USHORT processMachine, USHORT nativeMachine)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <IsWow64Process2 result=\"%d\">\n"), result);
    print_field_word(handle, TEXT("ProcessMachine"), processMachine);
    print_field_word(handle, TEXT("NativeMachine"), nativeMachine);
    _ftprintf(handle->out_file, TEXT("    </IsWow64Process2>\n"));
}

void print_product_info(writehandle_t *handle, BOOL result, DWORD dwOSMajor, DWORD dwOSMinor, DWORD dwSPMajor, DWORD dwSPMinor, DWORD dwProductType)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <GetProductInfo result=\"%d\" osMajor=\"%u\" osMinor=\"%u\" spMajor=\"%u\" spMinor=\"%u\">\n"), result, dwOSMajor, dwOSMinor, dwSPMajor, dwSPMinor);
    print_field_dword(handle, TEXT("ProductType"), dwProductType);
    _ftprintf(handle->out_file, TEXT("    </GetProductInfo>\n"));
}

void print_system_metric(writehandle_t *handle, int result, int nIndex)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <GetSystemMetrics result=\"%d\" nIndex=\"%d\" />\n"), result, nIndex);
}

void print_branding_string(writehandle_t *handle, LPCTSTR result, LPCTSTR format)
{
    if (!handle) return;

    _ftprintf(handle->out_file, TEXT("    <BrandingFormatString format=\"%s\">%s</BrandingFormatString>\n"), format, result);
}

static LPTSTR get_key(HKEY hKey)
{
    if (hKey == HKEY_LOCAL_MACHINE) {
        return TEXT("HKLM");
    } else if (hKey == HKEY_CURRENT_USER) {
        return TEXT("HKCU");
    } else {
        return NULL;
    }
}

static LPTSTR get_type(DWORD type)
{
    switch (type) {
    case REG_SZ: return TEXT("REG_SZ");
    case REG_DWORD: return TEXT("REG_DWORD");
    case REG_QWORD: return TEXT("REG_QWORD");
    case REG_BINARY: return TEXT("REG_BINARY");
    case REG_MULTI_SZ: return TEXT("REG_MULTI_SZ");
    default: return NULL;
    }
}

static void dump_registry_key_block(writehandle_t *handle, HKEY key)
{
    LSTATUS enumResult;
    DWORD index = 0;
    TCHAR name[256];
    DWORD type;
    BYTE data[1024];

    while (TRUE) {
        DWORD nameLen = 256;
        DWORD dataLen = 1024;
        LPTSTR typeStr;

        enumResult = RegEnumValue(key, index, name, &nameLen, NULL, &type, data, &dataLen);
        if (enumResult != ERROR_SUCCESS && enumResult != ERROR_MORE_DATA) {
            break;
        }

        typeStr = get_type(type);
        if (typeStr != NULL) {
            _ftprintf(handle->out_file, TEXT("      <Value name=\"%s\" type=\"%s\">"), name, typeStr);
        } else {
            _ftprintf(handle->out_file, TEXT("      <Value name=\"%s\" type=\"%d\">"), name, type);
        }
        switch (type) {
        case REG_SZ:
            _ftprintf(handle->out_file, TEXT("%s"), (TCHAR *)data);
            break;
        case REG_DWORD:
            _ftprintf(handle->out_file, TEXT("%lu"), *(DWORD *)data);
            break;
        case REG_QWORD:
            _ftprintf(handle->out_file, TEXT("%llu"), *(DWORDLONG *)data);
            break;
        default:
            break;
        }
        _ftprintf(handle->out_file, TEXT("</Value>\n"));
        index++;
    }
}

void dump_registry_key(writehandle_t *handle, HKEY hKey, LPTSTR subKey)
{
    HKEY key;
    LSTATUS result;

    if (!handle) return;

    result = RegOpenKeyEx(hKey, subKey, 0, KEY_READ, &key);
    if (result != ERROR_SUCCESS) {
        _ftprintf(handle->out_file, TEXT("    <Registry hive=\"%s\" key=\"%s\" result=\"%d\" />\n"), get_key(hKey), subKey, result);
        return;
    }

    _ftprintf(handle->out_file, TEXT("    <Registry hive=\"%s\" key=\"%s\">\n"), get_key(hKey), subKey);

    dump_registry_key_block(handle, key);

    _ftprintf(handle->out_file, TEXT("    </Registry>\n"));

    RegCloseKey(key);
}

static BOOL get_library_file_name(HMODULE hModule, LPTSTR path, LPDWORD pathLen)
{
    DWORD libPathLen;

    if (path == NULL || pathLen == NULL) return FALSE;

    libPathLen = GetModuleFileName(hModule, path, *pathLen);
    if (libPathLen == 0 || libPathLen == *pathLen) return FALSE;

    *pathLen = libPathLen;
    return TRUE;
}

struct file_version {
    WORD major;
    WORD minor;
    WORD revision;
    WORD build;
};
typedef struct file_version file_version_t;

static BOOL get_library_version(LPTSTR path, file_version_t *file, file_version_t *product)
{
    DWORD tHandle = 0;
    DWORD infoSize;
    VS_FIXEDFILEINFO *fileInfo = NULL;
    UINT len = 0;
    BYTE *buffer;

    infoSize = GetFileVersionInfoSize(path, &tHandle);
    if (infoSize == 0) return FALSE;

    buffer = (BYTE *)malloc(infoSize);
    if (buffer == NULL) return FALSE;

    if (!GetFileVersionInfo(path, 0, infoSize, buffer)) {
        free(buffer);
        return FALSE;
    }

    if (!VerQueryValue(buffer, TEXT("\\"), (LPVOID *)&fileInfo, &len)) {
        free(buffer);
        return FALSE;
    }

    if (file) {
        file->major = HIWORD(fileInfo->dwFileVersionMS);
        file->minor = LOWORD(fileInfo->dwFileVersionMS);
        file->revision = HIWORD(fileInfo->dwFileVersionLS);
        file->build = LOWORD(fileInfo->dwFileVersionLS);
    }

    if (product) {
        product->major = HIWORD(fileInfo->dwProductVersionMS);
        product->minor = LOWORD(fileInfo->dwProductVersionMS);
        product->revision = HIWORD(fileInfo->dwProductVersionLS);
        product->build = LOWORD(fileInfo->dwProductVersionLS);
    }

    free(buffer);
    return TRUE;
}

static LPTSTR get_file_name(LPTSTR path, DWORD pathLen)
{
    TCHAR *filePath = NULL;
    int i;

    if (path == NULL) return NULL;
    for (i = pathLen - 1; i > 0 && filePath == NULL; --i) {
        if (path[i] == TEXT('\\')) {
            filePath = &path[i + 1];
        }
    }
    return filePath;
}

void dump_library_version(writehandle_t *handle, HMODULE hModule)
{
    TCHAR path[MAX_PATH];
    DWORD pathLen = MAX_PATH;
    file_version_t file_version;
    file_version_t prod_version;
    LPTSTR filePath;

    if (!handle) return;

    if (!get_library_file_name(hModule, path, &pathLen)) return;

    if (!get_library_version(path, &file_version, &prod_version)) return;

    filePath = get_file_name(path, pathLen);
    if (filePath == NULL) return;

    _ftprintf(handle->out_file, TEXT("    <File path=\"%s\">\n"), filePath);
    _ftprintf(handle->out_file, TEXT("      <FileVersion major=\"%d\" minor=\"%d\" rev=\"%d\" build=\"%d\" />\n"),
        file_version.major,
        file_version.minor,
        file_version.revision,
        file_version.build);
    _ftprintf(handle->out_file, TEXT("      <ProductVersion major=\"%d\" minor=\"%d\" rev=\"%d\" build=\"%d\" />\n"),
        prod_version.major,
        prod_version.minor,
        prod_version.revision,
        prod_version.build);
    _ftprintf(handle->out_file, TEXT("    </File>\n"));
}
