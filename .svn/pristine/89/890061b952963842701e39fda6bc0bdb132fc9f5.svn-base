using System;
using Core.Mvvm;

namespace Core.Mvvm.Dialogs
{
    public interface IStringInputDialogService
    {
//        StringInputDialogArg DialogArg { get; }

        IObservable<DialogEventArgs<string>> Show(string title,string defaultString);
        IObservable<DialogEventArgs<string>> Show(string title,string defaultString,int maxLength);
    }
}