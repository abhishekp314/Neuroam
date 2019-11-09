using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuroam
{
    public class QueryDictionary
    {
        Dictionary<int, string> m_Queries = new Dictionary<int, string>();

        public QueryDictionary()
        {
            //TODO: Load the data from our json bin
        }

        public void Add(string query)
        {
            if (!m_Queries.ContainsValue(query))
            {
                m_Queries.Add(m_Queries.Count, query);
            }
        }

        public List<string> Find(string searchQuery)
        {
            List<string> searchResults = new List<string>();
            foreach (var query in m_Queries)
            {
                if (query.Value.Contains(searchQuery))
                {
                    searchResults.Add(query.Value);
                }
            }

            return searchResults;
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
            queryDictionary.Add("test 1");
            queryDictionary.Add("test 2");
            queryDictionary.Add("test 3");
            queryDictionary.Add("test 4");

            Assert.IsTrue(queryDictionary.Find("test").Count == 4);
        }
    }
    #endregion
}
