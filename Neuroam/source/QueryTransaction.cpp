#include <PrecompiledHeader.h>

#include <QueryTransaction.h>

namespace Neuroam {
bool QueryTransaction::ContainsWordId(long id) const
{
    return std::find(m_WordIds.begin(), m_WordIds.end(), id) != m_WordIds.end();
}
}
