#include <PrecompiledHeader.h>

#include <WordTransaction.h>

namespace Neuroam {

    const WordTransaction WordTransaction::InvalidWordTransaction = WordTransaction(-1, "");

bool WordTransaction::ContainsPartialMatch(long id) const
{
    return std::find(m_WordPartialMatches.begin(), m_WordPartialMatches.end(), id) != m_WordPartialMatches.end();
}

}