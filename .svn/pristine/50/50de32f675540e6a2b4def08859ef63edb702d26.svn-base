using System;
using System.Reactive;

namespace Core.Mvvm.Dialogs
{
    public interface IBusyDialogService: IClosable
    {
        IObservable<DialogEventArgs<Unit>> Show(string message);
    }
}