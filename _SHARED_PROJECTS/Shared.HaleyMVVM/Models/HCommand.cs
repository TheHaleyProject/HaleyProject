using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Windows;

namespace Haley.Models
{
   public class HCommand : HCommand<object>
    {
        public HCommand(Action<object> ActionMethod, Func<object, bool> ValidationFunction) :base(ActionMethod, ValidationFunction)
        {
        }

        public HCommand(Action<object> ActionMethod) : base(ActionMethod)
        {
        }
    }

    
    public abstract class HCommand<T> : ICommand
    {
        Action<T> _action_method;
        Func<object, bool> _validation_function;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            if (_validation_function == null) return true;
            return _validation_function.Invoke(parameter);
        }

        public virtual void Execute(object parameter)
        {
            _action_method?.Invoke((T)parameter);
        }

        public HCommand(Action<T> ActionMethod, Func<object, bool> ValidationFunction)
        {
            _action_method = ActionMethod;
            _validation_function = ValidationFunction;
        }

        public HCommand(Action<T> ActionMethod)
        {
            _action_method = ActionMethod;
            _validation_function = null;
        }
    }
}
