using System;
using System.Reactive;

namespace Core.Mvvm.Dialogs
{
    public interface IMessageDialogService: IClosable
    {
        IObservable<DialogEventArgs<Unit>> Show(string message);
    }
}