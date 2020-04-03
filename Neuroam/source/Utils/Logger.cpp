#include <PrecompiledHeader.h>

#include <Utils/Logger.h>

#include <windows.h>

namespace Neuroam {

void Logger(const char* format, ...)
{
    const unsigned int MaxCharBuffer = 1024;
    assert(MaxCharBuffer > strlen(format));

    char messageBuffer[MaxCharBuffer];

    va_list list;

    va_start(list, format);

    ::vsprintf_s(messageBuffer, format, list);
    strcat_s(messageBuffer, "\n");

    OutputDebugString(messageBuffer);

    va_end(list);
}

}