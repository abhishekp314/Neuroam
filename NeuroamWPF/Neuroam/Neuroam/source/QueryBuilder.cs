using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Neuroam
{
    public class QueryBuilder
    {
        WordDictionary m_WordDictionary;

        public QueryBuilder(WordDictionary wordDictionary)
        {
            m_WordDictionary = wordDictionary;
        }

        string[] TokenizeQueryIntoWords(string query)
        {
            // Default split by whitespace
            return query.Split(' ');
        }

        public QueryTransaction BuildQueryTransaction(string query)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(query));

            List<long> ids = new List<long>();
            foreach(var word in TokenizeQueryIntoWords(query))
            {
                ids.Add(m_WordDictionary.Add(word));
            }
            return new QueryTransaction(ids);
        }

        public string BuildQuery(List<long> ids)
        {
            string outQuery = "";
            WordTransaction transaction = null;
            foreach (var id in ids)
            {
                transaction = m_WordDictionary.Find(id);
                if (transaction != null)
                {
                    outQuery += " " + transaction.Word;
                }
            }
            return outQuery;
        }
    }
}
