using Neuroam.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuroam
{
    public class WordTransaction
    {
        public WordTransaction(long id, string word)
        {
            Id = id;
            Word = word;
            m_WordPartialMatches = new List<long>();
        }

        public long Id { get; set; }
        public string Word { get; set; }
        public List<long> m_WordPartialMatches { get; set; }
    }

    public class WordDictionary
    {
        List<WordTransaction> m_WordTransactions = new List<WordTransaction>();
        JsonFile m_WordDictionaryFile;
        public WordDictionary(bool inMemoryOnly = false)
        {
            m_WordDictionaryFile = new JsonFile(Constants.WordDictionaryFileName);
            string allData = m_WordDictionaryFile.ReadAll();

            if (!inMemoryOnly)
            {
                // Serialize the dictionary
                if (!string.IsNullOrWhiteSpace(allData))
                {
                    m_WordTransactions = JsonConvert.DeserializeObject<List<WordTransaction>>(allData);
                }
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
            string loweredWord = word.ToLower();
            return m_WordTransactions.Find(x => x.Word.ToLower() == loweredWord);
        }

        /// <summary>
        /// This function returns a partial match list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<long> FindPartialMatches(long id)
        {
            WordTransaction transaction = m_WordTransactions.Find(x => x.Id == id);
            if(transaction != null)
            {
                return transaction.m_WordPartialMatches;
            }

            return null;
        }

        /// <summary>
        /// This function returns a full match
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WordTransaction Find(long id)
        {
            List<WordTransaction> matches = new List<WordTransaction>();
            foreach (var transaction in m_WordTransactions)
            {
                if(transaction.Id == id)
                {
                    return transaction;
                }
            }

            return null;
        }

        public void BuildPartialMatches(WordTransaction newWordTranscation)
        {
            string loweredWord = newWordTranscation.Word.ToLower();
            foreach(var transcation in m_WordTransactions)
            {
                if(transcation.Word.ToLower().Contains(loweredWord))
                {
                    if(!transcation.m_WordPartialMatches.Contains(newWordTranscation.Id))
                    {
                        transcation.m_WordPartialMatches.Add(newWordTranscation.Id);
                    }

                    if (!newWordTranscation.m_WordPartialMatches.Contains(transcation.Id))
                    {
                        newWordTranscation.m_WordPartialMatches.Add(transcation.Id);
                    }
                }
            }
        }

        public long Add(string word)
        {
            long outId = -1;
            if (!string.IsNullOrWhiteSpace(word))
            {
                string normalizedWord = NormalizeWord(word);
                WordTransaction exists = Find(normalizedWord);
                if (exists == null)
                {
                    outId = GetNextId();
                    WordTransaction newWord = new WordTransaction(outId, normalizedWord);
                    BuildPartialMatches(newWord);
                    m_WordTransactions.Add(newWord);
                }
                else
                {
                    outId = exists.Id;
                }
            }
            return outId;
        }

        public void OnClose()
        {
            Logger.Instance.Log("Flushing WordDictionary Data");

            if(m_WordDictionaryFile != null)
            {
                // Write all the data to the file
                string jsonData = JsonConvert.SerializeObject(m_WordTransactions);
                m_WordDictionaryFile.WriteAll(jsonData);
            }
        }
    }
}
