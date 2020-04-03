#pragma once

namespace Neuroam {
class WordTransaction {
private:
    std::vector<long> m_WordPartialMatches;
    String m_Word = "";
    long m_Id = -1;

public:
    WordTransaction(long id, const String& word)
        : m_Word(word)
        , m_Id(id)
    {
    }

    inline long GetId() const { return m_Id; }
    inline StringView GetWord() const { return m_Word.ToString(); }

    inline void AddPartialMatch(long id) { m_WordPartialMatches.push_back(id); }
    inline const std::vector<long>& GetPartialMatches() const { return m_WordPartialMatches; }
    bool ContainsPartialMatch(long id) const;

    static const WordTransaction InvalidWordTransaction;
};

using WordTransactions = std::vector<WordTransaction>;
}