#pragma once

namespace Neuroam {

class JsonFile {
private:
    String m_FileName;

public:
    JsonFile(StringView fileName);

    StringView ReadAll();
    void WriteAll(StringView data);
};

}