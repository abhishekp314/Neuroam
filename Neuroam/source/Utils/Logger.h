#pragma once

#include <assert.h>

namespace Neuroam {

#define Assert(condition, msg, ...) \
    if (!condition) {               \
        Logger(msg, __VA_ARGS__);   \
        assert(condition);          \
    }

void Logger(const char* format, ...);

}