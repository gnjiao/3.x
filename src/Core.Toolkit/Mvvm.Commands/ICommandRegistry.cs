using System;
using System.Windows.Input;

namespace Core.Mvvm.Commands
{
    public interface ICommandRegistry
    {
        ICommand Add(Action executeFn);
        ICommand Add(Action executeFn, Func<object, bool> canExecuteFn);
        ICommand Add(Action<object> executeFn);
        ICommand Add(Action<object> executeFn, Func<object, bool> canExecuteFn);
        void RaiseCanExecuteChanged();
    }
}