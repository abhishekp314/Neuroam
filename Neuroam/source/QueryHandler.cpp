#include <PrecompiledHeader.h>

#include <QueryHandler.h>

namespace Neuroam {
QueryHandler::QueryHandler(bool inMemory)
{
    m_QueryDictionary = new QueryDictionary(inMemory);
}

QueryHandler::~QueryHandler()
{
    delete m_QueryDictionary;
}

void QueryHandler::OnSearchChanged(StringView inFlightQuery)
{
    if (!m_CurrentQuery.Equals(inFlightQuery))
    {
        // Tokenize and search again
        m_CurrentQuery = inFlightQuery;
    }
}

void QueryHandler::OnFinalizeQuery()
{
    m_QueryDictionary->Add(m_CurrentQuery.GetRaw());
    Strings results;
    if(m_QueryDictionary->Find(m_CurrentQuery.GetRaw(), results))
    {
        for (const auto& result : results) {
            Logger("%s", result.ToString().data());
        }
    }
    
}

void QueryHandler::OnClose()
{
    m_QueryDictionary->OnClose();
}
}