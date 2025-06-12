#ifndef WIN32_DUMP_H
#define WIN32_DUMP_H

#include <Windows.h>

struct writehandle;
typedef struct writehandle writehandle_t;

writehandle_t *init_write(LPCTSTR file_name);
void term_write(writehandle_t *handle);

void print_header(writehandle_t *handle);
void print_trailer(writehandle_t *handle);

void print_system_info(writehandle_t *handle, TCHAR *api, LPSYSTEM_INFO lpSystemInfo);
void print_version(writehandle_t *handle, DWORD value);
void print_version_info(writehandle_t *handle, BOOL result, LPOSVERSIONINFO lpVersionInformation);
void print_version_info_ex(writehandle_t *handle, BOOL result, LPOSVERSIONINFOEX lpVersionInformation);
void print_version_info_nt(writehandle_t *handle, NTSTATUS result, LPOSVERSIONINFOEX lpVersionInformation);
void print_is_wow64_process(writehandle_t *handle, BOOL result, BOOL value);
void print_is_wow64_process2(writehandle_t *handle, BOOL result, USHORT processMachine, USHORT nativeMachine);
void print_product_info(writehandle_t *handle, BOOL result, DWORD dwOSMajor, DWORD dwOSMinor, DWORD dwSPMajor, DWORD dwSPMinor, DWORD dwProductType);
void print_system_metric(writehandle_t *handle, int result, int nIndex);
void print_branding_string(writehandle_t *handle, LPCTSTR result, LPCTSTR format);

void dump_registry_key(writehandle_t *handle, HKEY hKey, LPTSTR subKey);

void dump_library_version(writehandle_t *handle, HMODULE hModule);

#endif
