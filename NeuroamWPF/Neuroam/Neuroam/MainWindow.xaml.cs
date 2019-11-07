using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Neuroam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> m_Items = new List<string>();
        private QueryHandler m_QueryHandler;

        public MainWindow()
        {
            InitializeComponent();

            m_QueryHandler = new QueryHandler(ResultsListBox);

            KeyBinding OpenCmdKeybinding = new KeyBinding(MainTextBox_EnterKeyCommand, Key.Enter, ModifierKeys.None);
            MainTextBox.InputBindings.Add(OpenCmdKeybinding);
            MainTextBox.TextChanged += MainTextBox_TextChanged;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_QueryHandler.OnSearchChanged(MainTextBox.Text);
        }


        public class EnterKeyAction : ICommand
        {
            public event EventHandler CanExecuteChanged;
            QueryHandler m_QueryHandlerRef;

            public EnterKeyAction(QueryHandler queryhandler)
            {
                m_QueryHandlerRef = queryhandler;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                m_QueryHandlerRef.OnFinalizeQuery();
            }
        }

        private ICommand MainTextBox_EnterKeyCommand
        {
            get
            {
                return new EnterKeyAction(m_QueryHandler);
            }
        }

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
                if(!inFlightQuery.Equals(CurrentQuery))
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

        public class QueryDictionary
        {
            Dictionary<int, string> m_Queries = new Dictionary<int, string>();

            public QueryDictionary()
            {
                //TODO: Load the data from our json bin
            }

            public void Add(string query)
            {
                if(!m_Queries.ContainsValue(query))
                {
                    m_Queries.Add(m_Queries.Count, query);
                }
            }

            public List<string> Find(string searchQuery)
            {
                List<string> searchResults = new List<string>();
                foreach(var query in m_Queries)
                {
                    if(query.Value.Contains(searchQuery))
                    {
                        searchResults.Add(query.Value);
                    }
                }

                return searchResults;
            }
        }
    }
}
