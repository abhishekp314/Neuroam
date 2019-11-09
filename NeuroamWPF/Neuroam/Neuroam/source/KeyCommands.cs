using System;
using System.Windows.Input;

namespace Neuroam
{
    public class EnterKeyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        QueryHandler m_QueryHandlerRef;

        public EnterKeyCommand(QueryHandler queryhandler)
        {
            m_QueryHandlerRef = queryhandler;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(CanExecute(parameter))
            {
                m_QueryHandlerRef.OnFinalizeQuery();
            }
        }
    }
}
