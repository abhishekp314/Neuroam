using Neuroam.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuroam
{
    class WordTransaction
    {
        public WordTransaction(long id, string word)
        {
            Id = id;
            Word = word;
        }

        public long Id { get; set; }
        public string Word { get; set; }
    }

    public class WordDictionary
    {
        List<WordTransaction> m_WordTransactions = new List<WordTransaction>();
        JsonFile m_WordDictionaryFile;
        public WordDictionary()
        {
            m_WordDictionaryFile = new JsonFile(Constants.WordDictionaryFileName);
            string allData = m_WordDictionaryFile.ReadAll();

            // Serialize the dictionary
            if(!string.IsNullOrWhiteSpace(allData))
            {
                m_WordTransactions = JsonConvert.DeserializeObject<List<WordTransaction>>(allData);
            }
        }

        long GetNextId()
        {
            return m_WordTransactions.LongCount();
        }

        string NormalizeWord(string word)
        {
            return word.Trim();
        }

        WordTransaction Find(string word)
        {
            return m_WordTransactions.Find(x => x.Word == word);
        }

        WordTransaction Find(long id)
        {
            return m_WordTransactions.Find(x => x.Id == id);
        }

        public long Process(string word)
        {
            string normalizedWord = NormalizeWord(word);
            WordTransaction exists = Find(normalizedWord);
            long outId;
            if (exists == null)
            {
                outId = GetNextId();
                m_WordTransactions.Add(new WordTransaction(outId, normalizedWord));
            }
            else
            {
                outId = exists.Id;
            }
            return outId;
        }

        public void OnClose()
        {
            // Write all the data to the file
            string jsonData = JsonConvert.SerializeObject(m_WordTransactions);
            m_WordDictionaryFile.WriteAll(jsonData);
        }
    }
}
