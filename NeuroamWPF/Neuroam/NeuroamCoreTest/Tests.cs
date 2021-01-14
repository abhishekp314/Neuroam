using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuroamCore;
using System;
using System.Collections.Generic;

namespace NeuroamCoreTest
{
    #region Test
    [TestClass]
    public class QueryDictionaryTest
    {
        QueryDictionary m_QueryDictionary = new QueryDictionary(true);

        [TestMethod]
        public void TestAdd()
        {
            m_QueryDictionary.Query("", true);
            Assert.IsTrue(m_QueryDictionary.Find("").Count == 0);

            m_QueryDictionary.Query("adder", true);
            m_QueryDictionary.Query("adder", true);

            Assert.IsTrue(m_QueryDictionary.Find("adder").Count == 1);
            Assert.IsTrue(m_QueryDictionary.Find("adder ").Count == 1);
        }

        [TestMethod]
        public void TestFind()
        {
            m_QueryDictionary.Query("unittest 1", true);
            m_QueryDictionary.Query("unittest 2", true);
            m_QueryDictionary.Query("unittest 3", true);
            m_QueryDictionary.Query("unittest 4", true);

            Assert.IsTrue(m_QueryDictionary.Find("unittest").Count == 4);
            Assert.IsTrue(m_QueryDictionary.Find("unittest 1").Count == 4);
        }

        [TestMethod]
        public void TestPartialFind()
        {
            m_QueryDictionary.Query("unittest 1", true);
            m_QueryDictionary.Query("unittest 2", true);
            m_QueryDictionary.Query("unittest 3", true);
            m_QueryDictionary.Query("unittest 4", true);

            Assert.IsTrue(m_QueryDictionary.Find("unit").Count == 4);
            Assert.IsTrue(m_QueryDictionary.Find("test").Count == 4);
        }

        [TestMethod]
        public void TestQueryOnly()
        {
            m_QueryDictionary.Query("unittest 1", true);
            m_QueryDictionary.Query("unittest 2");

            Assert.IsTrue(m_QueryDictionary.Find("unit").Count == 1);
            Assert.IsTrue(m_QueryDictionary.Find("test").Count == 1);
        }

        [TestMethod]
        public void TestRanking()
        {
            m_QueryDictionary.Query("Mein tera", true);
            m_QueryDictionary.Query("Bewajah sehta mein raha", true);
            m_QueryDictionary.Query("Kuch bhi", true);
            m_QueryDictionary.Query("Bewajah kehta mein raha", true);
            m_QueryDictionary.Query("oo oh oh yuh hin sehta raha", true);

            List<string> results = m_QueryDictionary.Find("sehta mein");
            Assert.IsTrue(results.Count == 4);
            Assert.IsTrue(results[0].Equals("Bewajah sehta mein raha", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(results[1].Equals("oo oh oh yuh hin sehta raha", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(results[2].Equals("Mein tera", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(results[3].Equals("Bewajah kehta mein raha", StringComparison.OrdinalIgnoreCase));


        }

        [TestMethod]
        public void TestRankingPartial()
        {
            m_QueryDictionary.Query("Mein tera", true);
            m_QueryDictionary.Query("Bewajah sehta mein raha", true);
            m_QueryDictionary.Query("Kuch bhi", true);
            m_QueryDictionary.Query("Bewajah kehta mein raha", true);
            m_QueryDictionary.Query("oo oh oh yuh hin sehta raha", true);

            List<string> results = m_QueryDictionary.Find("mei");
            Assert.IsTrue(results.Count == 3);
            Assert.IsTrue(results[0].Equals("Mein tera", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(results[1].Equals("Bewajah sehta mein raha", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(results[2].Equals("Bewajah kehta mein raha", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TestFindIDs()
        {
            m_QueryDictionary.Query("Mein tera", true);
            m_QueryDictionary.Query("Bewajah sehta mein raha", true);
            m_QueryDictionary.Query("Kuch bhi", true);
            m_QueryDictionary.Query("Bewajah kehta mein raha", true);
            m_QueryDictionary.Query("oo oh oh yuh hin sehta raha", true);

            List<long> results = m_QueryDictionary.FindQueryIds("mei");
            Assert.IsTrue(results.Count == 3);
            Assert.IsTrue(results[0] == 0);
            Assert.IsTrue(results[1] == 1);
            Assert.IsTrue(results[2] == 3);
        }

        [TestMethod]
        public void TestRankingRealTime()
        {
            m_QueryDictionary.Query("Anchor Switch", true);
            m_QueryDictionary.Query("Finnolex Wire", true);

            List<string> results = m_QueryDictionary.Find("anch wi");
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].Equals("Anchor Switch"));
            Assert.IsTrue(results[1].Equals("Finnolex Wire"));
        }
    }
    #endregion
}
