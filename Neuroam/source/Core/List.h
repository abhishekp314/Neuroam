#pragma once

//#include <Judy/Judy.h>

namespace Neuroam {

template <class T>
class List {
private:
    std::vector<T> m_Data;

public:
    List() { }

    inline void Add(const T& value)
    {
        m_Data.push_back(value);
    }

    inline bool IsEmpty() const { return m_Data.empty(); }
    inline size_t Length() const { return m_Data.size(); }
};

} // namespace Neuroam
