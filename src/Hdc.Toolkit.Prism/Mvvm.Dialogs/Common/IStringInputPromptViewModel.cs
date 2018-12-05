using System;
using Core.Mvvm.Dialogs;

namespace Hdc.Mvvm.Dialogs
{
    public interface IStringInputPromptViewModel
    {
        StringInputDialogArg DialogArg { get; }

        IObservable<DialogEventArgs<string>> ClosedEvent { get; }

        void Update(string title, string defaultString);
    }
}