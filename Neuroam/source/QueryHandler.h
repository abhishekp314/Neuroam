#pragma once

#include <QueryDictionary.h>

namespace Neuroam {
class QueryHandler {
private:
    // User search What's on your mind?
    String m_CurrentQuery;
    QueryDictionary* m_QueryDictionary;

public:
    QueryHandler(bool inMemory);
    ~QueryHandler();

    void OnSearchChanged(StringView inFlightQuery);
    void OnFinalizeQuery();
    void OnClose();
};
}
