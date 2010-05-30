using System;
using System.Windows.Input;

namespace SampleApplication.ViewModels
{
    public class SimpleCommand : ICommand
    {
        public Action<object> ExecuteDelegate { get; set; }
        public Func<object, bool> CanExecuteDelegate { get; set; }
        
       
        public void Execute(object parameter)
        {
            ExecuteDelegate.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}