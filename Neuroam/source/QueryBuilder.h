#pragma once

#include <QueryTransaction.h>
#include <WordDictionary.h>

namespace Neuroam {
class QueryBuilder {
public:
    QueryBuilder(WordDictionary& wordDictionary)
        : m_WordDictionary(wordDictionary)
    {
    }

    Strings TokenizeQueryIntoWords(StringView query) const;
    QueryTransaction BuildQueryTransaction(StringView query);
    String BuildQuery(const std::vector<long>& ids) const;

private:
    WordDictionary& m_WordDictionary;
};
}
