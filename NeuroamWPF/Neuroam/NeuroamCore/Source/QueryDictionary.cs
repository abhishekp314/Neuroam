using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuroamCore.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace NeuroamCore
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
        Timer m_SaveTimer;
        bool m_IsDictionaryDirty = false;

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

            // Create save timer that flushes the changes inmemory to disk when dirty
            m_SaveTimer = new Timer(Constants.SaveInterval);
            m_SaveTimer.Elapsed += OnSaveEvent;
            m_SaveTimer.AutoReset = true;
            m_SaveTimer.Enabled = true;
        }

        public void OnSaveEvent(Object source, ElapsedEventArgs e)
        {
            m_WordDictionary.Save();
            Save();
        }

        void Save()
        {
            if (m_QueryDictionaryFile != null && m_IsDictionaryDirty)
            {
                Logger.Instance.Log("Saving QueryDictionary to disk");
                string jsonData = JsonConvert.SerializeObject(m_Queries);
                m_QueryDictionaryFile.WriteAll(jsonData);
                m_IsDictionaryDirty = false;
            }
        }

        public long Query(string queryData, bool addQueryToDictionary = false)
        {
            long queryId = -1;
            if (!string.IsNullOrWhiteSpace(queryData))
            {
                QueryTransaction newQueryTransaction = m_QueryBuilder.BuildQueryTransaction(queryData);

                bool queryExists = false;
                for (int queryIndex = 0; queryIndex < m_Queries.Count; ++queryIndex)
                {
                    if (m_Queries[queryIndex].WordIds.SequenceEqual(newQueryTransaction.WordIds))
                    {
                        queryExists = true;
                        queryId = queryIndex;
                        break;
                    }
                }

                if (addQueryToDictionary && !queryExists)
                {
                    queryId = m_Queries.Count;
                    m_Queries.Add(newQueryTransaction);
                    m_IsDictionaryDirty = true;
                }
            }
            return queryId;
        }

        private class RankedTransaction
        {
            public RankedTransaction(QueryTransaction queryTransaction, long queryId)
            {
                QueryTranscationRef = queryTransaction;
                QueryId = queryId;
            }
            public QueryTransaction QueryTranscationRef;
            public int NumberOfMatchedWords = 0;
            public int NumberOfMatchedPartialCharacters = 0;
            public long QueryId = 0;
        }

        private List<RankedTransaction> FindInternal(string searchQuery)
        {
            List<RankedTransaction> rankedQueryTransactions = new List<RankedTransaction>();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                QueryTransaction searchQueryTransaction = m_QueryBuilder.BuildQueryTransaction(searchQuery);
                foreach (var searchWordId in searchQueryTransaction.WordIds)
                {
                    // Get partial search matches for the searchWordId
                    List<long> partialSearchMatches = m_WordDictionary.FindPartialMatches(searchWordId);

                    // Full and partial match strategy
                    for (int queryIndex = 0; queryIndex < m_Queries.Count; ++queryIndex)
                    {
                        var query = m_Queries[queryIndex];
                        // Full match
                        if (query.WordIds.Contains(searchWordId))
                        {
                            var rankedQuery = rankedQueryTransactions.Find(x => x.QueryTranscationRef == query);
                            if (rankedQuery == null)
                            {
                                rankedQuery = new RankedTransaction(query, queryIndex);
                                rankedQuery.NumberOfMatchedWords++;
                                rankedQueryTransactions.Add(rankedQuery);
                            }
                            else
                            {
                                rankedQuery.NumberOfMatchedWords++;
                            }
                        }
                        // Partial match
                        else
                        {
                            foreach (var queryWordId in query.WordIds)
                            {
                                if (partialSearchMatches.Contains(queryWordId))
                                {
                                    var rankedQuery = rankedQueryTransactions.Find(x => x.QueryTranscationRef == query);
                                    if (rankedQuery == null)
                                    {
                                        rankedQuery = new RankedTransaction(query, queryIndex);
                                        rankedQuery.NumberOfMatchedPartialCharacters++;
                                        rankedQueryTransactions.Add(rankedQuery);
                                    }
                                    else
                                    {
                                        rankedQuery.NumberOfMatchedPartialCharacters++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                rankedQueryTransactions.Sort(delegate (RankedTransaction x, RankedTransaction y)
                {
                    // Lower the number the x is pushed up the sort. 0 means no change
                    if (x.NumberOfMatchedWords == y.NumberOfMatchedWords)
                    {
                        if (x.NumberOfMatchedPartialCharacters > y.NumberOfMatchedPartialCharacters)
                            return 0;
                        else if (x.NumberOfMatchedPartialCharacters < y.NumberOfMatchedPartialCharacters)
                            return 1;
                    }
                    else if (x.NumberOfMatchedWords > y.NumberOfMatchedWords)
                        return 0;
                    else if (x.NumberOfMatchedWords < y.NumberOfMatchedWords)
                        return 1;
                    return 0;
                });
            }

            return rankedQueryTransactions;
        }

        public List<long> FindQueryIds(string searchQuery)
        {
            List<long> searchResultsIds = new List<long>();
            List<RankedTransaction> rankedQueryTransactions = FindInternal(searchQuery);
            foreach (var rankedQueryTransaction in rankedQueryTransactions)
            {
                searchResultsIds.Add(rankedQueryTransaction.QueryId);
            }
            return searchResultsIds;
        }

        public List<string> Find(string searchQuery)
        {
            List<string> searchResults = new List<string>();
            List<RankedTransaction> rankedQueryTransactions = FindInternal(searchQuery);
            foreach (var rankedQueryTransaction in rankedQueryTransactions)
            {
                searchResults.Add(m_QueryBuilder.BuildQuery(rankedQueryTransaction.QueryTranscationRef.WordIds));
            }
            return searchResults;
        }

        public void Clear()
        {
            m_WordDictionary.Clear();
            m_Queries.Clear();
        }

        public void OnClose()
        {
            m_WordDictionary.OnClose();

            Save();
        }
    }
}
