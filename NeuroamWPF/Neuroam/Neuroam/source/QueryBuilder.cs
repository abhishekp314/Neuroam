﻿using System;
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
            List<long> ids = new List<long>();
            foreach(var word in TokenizeQueryIntoWords(query))
            {
                ids.Add(m_WordDictionary.Process(word));
            }
            Debug.Assert(ids.Count > 0);
            return new QueryTransaction(ids);
        }
    }
}
