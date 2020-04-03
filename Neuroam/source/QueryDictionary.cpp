#include <PrecompiledHeader.h>

#include <QueryDictionary.h>

#include <QueryBuilder.h>
#include <QueryTransaction.h>
#include <WordDictionary.h>
#include <IO/JsonFile.h>

namespace Neuroam
{
    QueryDictionary::QueryDictionary(bool inMemoryOnly)
    {
        if (!inMemoryOnly)
        {
            // Serialize queries
            m_QueryDictionaryFile = new JsonFile(Constants::QueryDictionaryFileName);
            StringView allData = m_QueryDictionaryFile->ReadAll();
            if (!String::IsNullOrWhiteSpace(allData))
            {
                //m_Queries = JsonConvert.DeserializeObject<List<QueryTransaction>>(allData);
            }
        }

        m_WordDictionary = new WordDictionary(inMemoryOnly);
        m_QueryBuilder = new QueryBuilder(*m_WordDictionary);
    }

    QueryDictionary::~QueryDictionary()
    {
        delete m_WordDictionary;
        delete m_QueryBuilder;
        delete m_QueryDictionaryFile;
    }

    void QueryDictionary::Add(StringView queryData)
    {
        if (!String::IsNullOrWhiteSpace(queryData))
        {
            QueryTransaction newQueryTransaction = m_QueryBuilder->BuildQueryTransaction(queryData);

            bool queryExists = false;
            for (const auto& query : m_Queries)
            {
                if (query.GetWordIds() == newQueryTransaction.GetWordIds())
                {
                    queryExists = true;
                    break;
                }
            }

            if (!queryExists)
            {
                m_Queries.push_back(newQueryTransaction);
            }
        }
    }

    bool QueryDictionary::Find(StringView searchQuery, Strings& searchResults)
    {
        if (!String::IsNullOrWhiteSpace(searchQuery))
        {
            QueryTransaction searchQueryTransaction = m_QueryBuilder->BuildQueryTransaction(searchQuery);
            std::vector<const QueryTransaction*> matchedQueryTransactions;

            for (const auto& searchWordId : searchQueryTransaction.GetWordIds())
            {
                // Get partial search matches for the searchWordId
                const std::vector<long>& partialSearchMatches = m_WordDictionary->FindPartialMatches(searchWordId);

                // Full and partial match strategy
                for (const auto& query : m_Queries)
                {
                    bool matchFound = false;

                    // Full match
                    if (query.ContainsWordId(searchWordId))
                    {
                        matchFound = true;
                    }
                    else
                    {
                        // Partial match
                        for (const auto& queryWordId : query.GetWordIds())
                        {
                            if (std::find(partialSearchMatches.begin(), partialSearchMatches.end(), queryWordId) != partialSearchMatches.end())
                            {
                                matchFound = true;
                                break;
                            }
                        }
                    }

                    if (matchFound && std::find(matchedQueryTransactions.begin(), matchedQueryTransactions.end(), &query) == matchedQueryTransactions.end())
                    {
                        String search = m_QueryBuilder->BuildQuery(query.GetWordIds());
                        searchResults.push_back(search);
                        matchedQueryTransactions.push_back(&query);
                    }
                }
            }
        }
        return searchResults.size() > 0;
    }

    void QueryDictionary::OnClose()
    {
        m_WordDictionary->OnClose();

        if (m_QueryDictionaryFile != nullptr)
        {
            Logger("Flushing QueryDictionary Data");
            //String jsonData = JsonConvert.SerializeObject(m_Queries);
            //m_QueryDictionaryFile.WriteAll(jsonData);
        }
    }
}