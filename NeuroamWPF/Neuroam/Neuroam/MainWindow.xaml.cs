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

        public MainWindow()
        {
            InitializeComponent();
            MainTextBox.TextChanged += MainTextBox_TextChanged;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResultsListBox.ItemsSource = MainTextBox.Text;
        }
    }
}
