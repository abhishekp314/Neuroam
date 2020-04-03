#pragma once

#include <WordTransaction.h>

namespace Neuroam {
class JsonFile;
class WordDictionary {

private:
    WordTransactions m_WordTransactions;
    JsonFile* m_WordDictionaryFile;

public:
    WordDictionary(bool inMemory = false);
    ~WordDictionary();

    const WordTransaction& Find(StringView word) const;
    const WordTransaction& Find(long id) const;
    const std::vector<long>& FindPartialMatches(long id) const;

    void BuildPartialMatches(WordTransaction& newWordTranscation);
    long Add(StringView word);
    void OnClose();

private:
    void RebuildPartialMatches();
    long GetNextId();
    void NormalizeWord(String& word) const;
};
}