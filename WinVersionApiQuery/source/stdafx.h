//#define WINVER 0x0400

#include <Windows.h>

#ifndef WIN32_COMPAT_H
#define WIN32_COMPAT_H

// Compilation using Visual Studio '98 or earlier
#if WINVER <= 0x0400

// VS2005 defines this as:
//  typedef _W64 unsigned long ULONG_PTR, *PULONG_PTR;
//  typedef ULONG_PTR DWORD_PTR, *PDWORD_PTR
//
// Any typedef that has __w64 on it must be 32 bits on x86 and 64 bits on
// Itanium Processor Family (IPF).
typedef unsigned long DWORD_PTR;

// Defined on VS2005 for 64-bit. On 32-bit machines, the registry functions
// will *not* return a type of REG_QWORD. Neither does any 32-bit binary, e.g.
// when running on 64-bit windows under WoW32.
#define REG_QWORD 11

#endif


// Compilation using Visual Studio 2005 or earlier
#if WINVER <= 0x0501

// This type isn't defined on Visual Studio '98.
typedef LONG NTSTATUS;
typedef LONG LSTATUS;

// Appears not to be defined in Visual Studio 2005
#ifndef SM_TABLEPC
#define SM_TABLETPC             86
#endif
#ifndef SM_MEDIACENTER
#define SM_MEDIACENTER          87
#endif
#ifndef SM_STARTER
#define SM_STARTER              88
#endif
#ifndef SM_SERVERR2
#define SM_SERVERR2             89
#endif

#endif   WINVER <= 0x0501

#endif