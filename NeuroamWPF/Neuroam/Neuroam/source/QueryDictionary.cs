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

        public QueryDictionary()
        {
            m_Queries = new List<QueryTransaction>();

            // Serialize queries
            m_QueryDictionaryFile = new JsonFile(Constants.QueryDictionaryFileName);
            string allData = m_QueryDictionaryFile.ReadAll();
            if (!string.IsNullOrWhiteSpace(allData))
            {
                m_Queries = JsonConvert.DeserializeObject<List<QueryTransaction>>(allData);
            }

            m_WordDictionary = new WordDictionary();
            m_QueryBuilder = new QueryBuilder(m_WordDictionary);
        }

        public void Add(string query)
        {
            QueryTransaction newQueryTransaction = m_QueryBuilder.BuildQueryTransaction(query);

            if (m_Queries.Find(x => x.WordIds == newQueryTransaction.WordIds) == null)
            {
                m_Queries.Add(newQueryTransaction);
            }
        }

        public List<string> Find(string searchQuery)
        {
            QueryTransaction searchQueryTransaction = m_QueryBuilder.BuildQueryTransaction(searchQuery);
            List<string> searchResults = new List<string>();
            foreach (var query in m_Queries)
            {
                foreach(var id in searchQueryTransaction.WordIds)
                {
                    if(query.WordIds.Contains(id))
                    {
                        searchResults.Add(m_QueryBuilder.BuildQuery(query.WordIds));
                    }
                }
            }

            return searchResults;
        }

        public void OnClose()
        {
            m_WordDictionary.OnClose();

            Logger.Instance.Log("Flushing QueryDictionary Data");
            string jsonData = JsonConvert.SerializeObject(m_Queries);
            m_QueryDictionaryFile.WriteAll(jsonData);
        }
    }

    #region Test
    [TestClass]
    public class QueryDictionaryTest
    {
        [TestMethod]
        public void TestFind()
        {
            QueryDictionary queryDictionary = new QueryDictionary();
            queryDictionary.Add("unittest 1");
            queryDictionary.Add("unittest 2");
            queryDictionary.Add("unittest 3");
            queryDictionary.Add("unittest 4");

            Assert.IsTrue(queryDictionary.Find("unittest").Count == 4);
        }
    }
    #endregion
}
