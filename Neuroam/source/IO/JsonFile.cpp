#include <PrecompiledHeader.h>

#include <IO/JsonFile.h>

namespace Neuroam {

JsonFile::JsonFile(StringView fileName)
{
    m_FileName = fileName;
}

StringView JsonFile::ReadAll()
{
    return "";
}

void JsonFile::WriteAll(StringView)
{
}

}