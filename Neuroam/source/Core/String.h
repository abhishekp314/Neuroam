#pragma once

namespace Neuroam {

using StringView = std::string_view;
using Strings = std::vector<class String>;

class String {
private:
    std::string m_Data = "";

public:
    String() {}

    String(const String& value)
        : m_Data(value.ToString())
    {
    }

    String(const char* value)
        : m_Data(value)
    {
    }

    String(StringView value)
        : m_Data(value)
    {
    }

    void operator=(const String& obj)
    {
        m_Data = obj.GetRaw();
    }

    void operator=(StringView obj)
    {
        m_Data = obj;
    }

    String& operator+=(const String& str)
    {
        m_Data += str.GetRaw();
        return *this;
    }

    String& operator+=(StringView str)
    {
        m_Data += str;
        return *this;
    }

    String operator+(StringView str)
    {
        String data = *this;
        data += str;
        return data;
    }

    inline const std::string& GetRaw() const { return m_Data; }
    inline StringView ToString() const { return m_Data.c_str(); }
    inline size_t Length() const { return m_Data.length(); }
    inline bool Equals(StringView view) const { return m_Data == view; }

    inline StringView ToLower()
    {
        std::transform(m_Data.begin(), m_Data.end(), m_Data.begin(),
            [](unsigned char c) { return towlower(c); });
        return m_Data;
    }

    inline void Trim()
    {
        m_Data.erase(m_Data.begin(), std::find_if(m_Data.begin(), m_Data.end(), [](int c) { return !iswspace(c); }));
    }

    inline bool Contains(const String& value)
    {
        return Contains(value.ToString());
    }
    bool Contains(StringView value)
    {
        return m_Data.find(value) != std::string::npos;
    }

    // Static
    static inline bool IsNullOrWhiteSpace(StringView value) { return value.size() == 0 || value[0] == ' '; }
    static inline Strings Split(StringView data, StringView delimiter)
    {
        Strings outSplits;
        size_t pos = 0;
        std::string token;
        std::string s(data);
        while ((pos = s.find(delimiter)) != std::string::npos) {
            token = s.substr(0, pos);
            outSplits.push_back(token.c_str());
            s.erase(0, pos + delimiter.length());
        }

        // Add the last string if exists
        if(s.length() > 0)
        {
            outSplits.push_back(s.c_str());
        }
        return outSplits;
    }
};
}