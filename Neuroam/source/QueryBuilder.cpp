#include <PrecompiledHeader.h>

#include <QueryBuilder.h>

namespace Neuroam
{
    Strings QueryBuilder::TokenizeQueryIntoWords(StringView query) const
    {
        // Default split by whitespace
        return String::Split(query, " ");
    }

    QueryTransaction QueryBuilder::BuildQueryTransaction(StringView query)
    {
        std::vector<long> ids;
        for (const auto& word : TokenizeQueryIntoWords(query))
        {
            ids.push_back(m_WordDictionary.Add(word.ToString()));
        }
        return QueryTransaction(ids);
    }

    String QueryBuilder::BuildQuery(const std::vector<long>& ids) const
    {
        String outQuery("");
        WordTransaction transaction = WordTransaction::InvalidWordTransaction;
        for (auto id : ids)
        {
            transaction = m_WordDictionary.Find(id);
            if (&transaction != &WordTransaction::InvalidWordTransaction)
            {
                outQuery += String(" ") + transaction.GetWord();
            }
        }
        outQuery.Trim();
        return outQuery;
    }
}