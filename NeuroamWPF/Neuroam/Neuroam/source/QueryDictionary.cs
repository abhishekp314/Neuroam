using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuroam.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuroam
{
    public class QueryTransaction
    {
        public QueryTransaction(List<long> ids)
        {
            TimeStamp = DateTime.Now;
            WordIds = ids;
            Weight = 0.0f;
        }

        public DateTime TimeStamp { get; set; }
        public List<long> WordIds { get; set; }
        public float Weight { get; set; }
    }

    public class QueryDictionary
    {
        List<QueryTransaction> m_Queries;
        WordDictionary m_WordDictionary;
        QueryBuilder m_QueryBuilder;
        JsonFile m_QueryDictionaryFile;

        public QueryDictionary(bool inMemoryOnly = false)
        {
            m_Queries = new List<QueryTransaction>();

            if (!inMemoryOnly)
            {
                // Serialize queries
                m_QueryDictionaryFile = new JsonFile(Constants.QueryDictionaryFileName);
                string allData = m_QueryDictionaryFile.ReadAll();
                if (!string.IsNullOrWhiteSpace(allData))
                {
                    m_Queries = JsonConvert.DeserializeObject<List<QueryTransaction>>(allData);
                }
            }

            m_WordDictionary = new WordDictionary(inMemoryOnly);
            m_QueryBuilder = new QueryBuilder(m_WordDictionary);
        }

        public void Add(string queryData)
        {
            if (!string.IsNullOrWhiteSpace(queryData))
            {
                QueryTransaction newQueryTransaction = m_QueryBuilder.BuildQueryTransaction(queryData);

                bool queryExists = false;
                foreach (var query in m_Queries)
                {
                    if(query.WordIds.SequenceEqual(newQueryTransaction.WordIds))
                    {
                        queryExists = true;
                        break;
                    }
                }

                if (!queryExists)
                {
                    m_Queries.Add(newQueryTransaction);
                }
            }
        }

        public List<string> Find(string searchQuery)
        {
            List<string> searchResults = new List<string>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                QueryTransaction searchQueryTransaction = m_QueryBuilder.BuildQueryTransaction(searchQuery);

                foreach (var searchWordId in searchQueryTransaction.WordIds)
                {
                    // Get partial search matches for the searchWordId
                    List<long> partialSearchMatches = m_WordDictionary.FindPartialMatches(searchWordId);

                    // Full and partial match strategy
                    foreach (var query in m_Queries)
                    {
                        bool matchFound = false;

                        // Full match
                        if (query.WordIds.Contains(searchWordId))
                        {
                            matchFound = true;
                        }
                        else
                        {
                            // Partial match
                            foreach (var queryWordId in query.WordIds)
                            {
                                if (partialSearchMatches.Contains(queryWordId))
                                {
                                    matchFound = true;
                                    break;
                                }
                            }
                        }

                        if (matchFound)
                            searchResults.Add(m_QueryBuilder.BuildQuery(query.WordIds));
                    }
                }
            }

            return searchResults;
        }

        public void OnClose()
        {
            m_WordDictionary.OnClose();

            if (m_QueryDictionaryFile != null)
            {
                Logger.Instance.Log("Flushing QueryDictionary Data");
                string jsonData = JsonConvert.SerializeObject(m_Queries);
                m_QueryDictionaryFile.WriteAll(jsonData);
            }
        }
    }

    #region Test
    [TestClass]
    public class QueryDictionaryTest
    {
        QueryDictionary m_QueryDictionary = new QueryDictionary(true);

        [TestMethod]
        public void TestAdd()
        {
            m_QueryDictionary.Add("");
            Assert.IsTrue(m_QueryDictionary.Find("").Count == 0);

            m_QueryDictionary.Add("adder");
            m_QueryDictionary.Add("adder");

            Assert.IsTrue(m_QueryDictionary.Find("adder").Count == 1);
        }

        [TestMethod]
        public void TestFind()
        {
            m_QueryDictionary.Add("unittest 1");
            m_QueryDictionary.Add("unittest 2");
            m_QueryDictionary.Add("unittest 3");
            m_QueryDictionary.Add("unittest 4");

            Assert.IsTrue(m_QueryDictionary.Find("unittest").Count == 4);
        }

        [TestMethod]
        public void TestPartialFind()
        {
            m_QueryDictionary.Add("unittest 1");
            m_QueryDictionary.Add("unittest 2");
            m_QueryDictionary.Add("unittest 3");
            m_QueryDictionary.Add("unittest 4");

            Assert.IsTrue(m_QueryDictionary.Find("unit").Count == 4);
            Assert.IsTrue(m_QueryDictionary.Find("test").Count == 4);
        }
    }
    #endregion
}
