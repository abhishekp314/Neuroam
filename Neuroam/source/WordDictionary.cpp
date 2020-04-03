#include <PrecompiledHeader.h>

#include <WordDictionary.h>
#include <IO/JsonFile.h>

namespace Neuroam
{
    WordDictionary::WordDictionary(bool inMemoryOnly)
    {
        m_WordDictionaryFile = new JsonFile(Constants::WordDictionaryFileName);
        StringView allData = m_WordDictionaryFile->ReadAll();

        if (!inMemoryOnly)
        {
            // Serialize the dictionary
            if (!String::IsNullOrWhiteSpace(allData))
            {
                //m_WordTransactions = JsonConvert.DeserializeObject<WordTransactions>(allData);
            }
        }

        RebuildPartialMatches();
    }

    WordDictionary::~WordDictionary()
    {
        delete m_WordDictionaryFile;
    }

    const WordTransaction& WordDictionary::Find(StringView word) const
    {
        String loweredWord = word;
        loweredWord.ToLower();

        auto it = std::find_if(m_WordTransactions.begin(), m_WordTransactions.end(), 
            [loweredWord](const WordTransaction& wordTrans) { return wordTrans.GetWord() == loweredWord.ToString(); });
        if (it != m_WordTransactions.end()) 
        {
            return *it;
        }
        return WordTransaction::InvalidWordTransaction;
    }

    const WordTransaction& WordDictionary::Find(long id) const
    {
        WordTransactions matches;
        for (const auto& transaction : m_WordTransactions)
        {
            if (transaction.GetId() == id)
            {
                return transaction;
            }
        }

        return WordTransaction::InvalidWordTransaction;
    }

    const std::vector<long>& WordDictionary::FindPartialMatches(long id) const
    {
        auto it = std::find_if(m_WordTransactions.begin(), m_WordTransactions.end(), 
            [id](const auto& trans) { return id == trans.GetId(); });
        if (it != m_WordTransactions.end())
        {
            return (*it).GetPartialMatches();
        }

        return WordTransaction::InvalidWordTransaction.GetPartialMatches();
    }

    void WordDictionary::BuildPartialMatches(WordTransaction& newWordTranscation)
    {
        String loweredWord = newWordTranscation.GetWord();
        loweredWord.ToLower();
        for(auto transcation : m_WordTransactions)
        {
            String transcationWordLowered = transcation.GetWord();
            transcationWordLowered.ToLower();

            if (transcationWordLowered.Contains(loweredWord) || loweredWord.Contains(transcationWordLowered))
            {
                if (!transcation.ContainsPartialMatch(newWordTranscation.GetId()))
                {
                    transcation.AddPartialMatch(newWordTranscation.GetId());
                }

                if (!newWordTranscation.ContainsPartialMatch(transcation.GetId()))
                {
                    newWordTranscation.AddPartialMatch(transcation.GetId());
                }
            }
        }
    }

    long WordDictionary::Add(StringView word)
    {
        long outId = -1;
        if (!String::IsNullOrWhiteSpace(word))
        {
            String normalizedWord = word;
            NormalizeWord(normalizedWord);

            const WordTransaction& exists = Find(normalizedWord.ToString());
            if (&exists == &WordTransaction::InvalidWordTransaction)
            {
                outId = GetNextId();
                WordTransaction newWord(outId, normalizedWord);
                BuildPartialMatches(newWord);
                m_WordTransactions.push_back(newWord);
            }
            else
            {
                outId = exists.GetId();
            }
        }
        return outId;
    }

    void WordDictionary::OnClose()
    {
        Logger("Flushing WordDictionary Data");

        if (m_WordDictionaryFile != nullptr)
        {
            // Write all the data to the file
            //String jsonData = JsonConvert.SerializeObject(m_WordTransactions);
            //m_WordDictionaryFile->WriteAll(jsonData);
        }
    }

    void WordDictionary::RebuildPartialMatches()
    {
        for (auto& wordTransaction : m_WordTransactions)
        {
            BuildPartialMatches(wordTransaction);
        }
    }

    long WordDictionary::GetNextId()
    {
        return m_WordTransactions.size();
    }

    void WordDictionary::NormalizeWord(String& word) const
    {
        word.Trim();
    }
}