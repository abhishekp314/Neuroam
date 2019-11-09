using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                // Tokenize and search again
                CurrentQuery = inFlightQuery;
            }
        }
        public void OnFinalizeQuery()
        {
            m_QueryDictionary.Add(CurrentQuery);
            m_ResultsListBox.ItemsSource = m_QueryDictionary.Find(CurrentQuery);
        }
    }
}
