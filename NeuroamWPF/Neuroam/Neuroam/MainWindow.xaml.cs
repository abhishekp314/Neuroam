using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Neuroam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region MainWindow
        
        private QueryHandler m_QueryHandler;

        public MainWindow()
        {
            InitializeComponent();
            InitializeNeuroam();
        }

        private void InitializeNeuroam()
        {
            m_QueryHandler = new QueryHandler(ResultsListBox);

            InitializeNeuroamUIElements();
        }

        private void InitializeNeuroamUIElements()
        {
            // MainTextBox
            KeyBinding OpenCmdKeybinding = new KeyBinding(MainTextBox_EnterKeyCommand, Key.Enter, ModifierKeys.None);
            MainTextBox.InputBindings.Add(OpenCmdKeybinding);
            MainTextBox.TextChanged += MainTextBox_TextChanged;
        }

        #endregion


        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_QueryHandler.OnSearchChanged(MainTextBox.Text);
        }

        private ICommand MainTextBox_EnterKeyCommand
        {
            get
            {
                return new EnterKeyCommand(m_QueryHandler);
            }
        }
    }
}
