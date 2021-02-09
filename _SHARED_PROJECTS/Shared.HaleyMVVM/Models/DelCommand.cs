﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Windows;

namespace Haley.Models
{
   public class DelCommand : DelCommand<object>
    {
        public DelCommand(Action<object> ActionMethod, Func<object, bool> ValidationFunction) :base(ActionMethod, ValidationFunction)
        {
        }

        public DelCommand(Action<object> ActionMethod) : base(ActionMethod)
        {
        }
    }

    
    public class DelCommand<T> : ICommand
    {
        Action<T> _action_method;
        Func<T, bool> _validation_function;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            if (_validation_function == null) return true;
            T _param = (parameter != null) ? (T)parameter : default(T);
            return _validation_function.Invoke(_param);
        }

        public virtual void Execute(object parameter)
        {
            _action_method?.Invoke((T)parameter);
        }

        public DelCommand(Action<T> ActionMethod, Func<T, bool> ValidationFunction)
        {
            _action_method = ActionMethod;
            _validation_function = ValidationFunction;
        }

        public DelCommand(Action<T> ActionMethod)
        {
            _action_method = ActionMethod;
            _validation_function = null;
        }
    }
}
