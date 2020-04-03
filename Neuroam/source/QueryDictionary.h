#pragma once

#include <QueryTransaction.h>

namespace Neuroam {
class JsonFile;
class QueryBuilder;
class WordDictionary;

class QueryDictionary {
private:
    QueryBuilder* m_QueryBuilder = nullptr;
    JsonFile* m_QueryDictionaryFile = nullptr;
    WordDictionary* m_WordDictionary = nullptr;
    QueryTransactions m_Queries;

public:
    QueryDictionary(bool inMemoryOnly = false);
    ~QueryDictionary();

    void Add(StringView queryData);
    bool Find(StringView searchQuery, Strings& outSearchResults);
    void OnClose();
};
}
