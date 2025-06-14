#ifndef WIN32_COMPAT_H
#define WIN32_COMPAT_H

#include <Windows.h>

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
