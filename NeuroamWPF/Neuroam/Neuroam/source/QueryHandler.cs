using System.Windows.Controls;
using NeuroamCore;

namespace Neuroam
{
    public class QueryHandler
    {
        // User search What's on your mind?
        string CurrentQuery { get; set; }
        ListBox m_ResultsListBox;
        QueryDictionary m_QueryDictionary = new QueryDictionary();

        public QueryHandler(ListBox resultListBox)
        {
            m_ResultsListBox = resultListBox;
        }

        public void OnSearchChanged(string inFlightQuery)
        {
            if (!inFlightQuery.Equals(CurrentQuery))
            {
                CurrentQuery = inFlightQuery;
                m_QueryDictionary.Query(CurrentQuery);
                m_ResultsListBox.ItemsSource = m_QueryDictionary.Find(CurrentQuery);
            }
        }
        public void OnFinalizeQuery()
        {
            m_QueryDictionary.Query(CurrentQuery, true /*Add this query to the dictionary*/);
            m_ResultsListBox.ItemsSource = m_QueryDictionary.Find(CurrentQuery);
        }

        public void OnClose()
        {
            m_QueryDictionary.OnClose();
        }
    }
}
