using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Windows;

#pragma warning disable IDE1006 // Naming Styles
namespace Haley.Models
{
   public class HCommand : ICommand
    {

        Action<object> _method_to_execute;
        Func<object, bool> _function_to_check;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_function_to_check == null) return true;
            return _function_to_check.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _method_to_execute?.Invoke(parameter);
        }

        public HCommand(Action<object> ActualMethodToExecute, Func<object, bool> ActualFunctionToCheck)
        {
            _method_to_execute = ActualMethodToExecute;
            _function_to_check = ActualFunctionToCheck;
        }

        public HCommand(Action<object> ActualMethodToExecute)
        {
            _method_to_execute = ActualMethodToExecute;
            _function_to_check = null;
        }
    }

}
#pragma warning restore IDE1006 // Naming Styles