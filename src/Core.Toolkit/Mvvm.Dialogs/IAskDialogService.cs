using System;
using System.Windows;
using Core.Mvvm;

namespace Core.Mvvm.Dialogs
{
    public interface IAskDialogService: IGeneralInputOutputDialogService<string, bool>
    {
        //IObservable<DialogEventArgs<bool>> Show(string question);
    }
}