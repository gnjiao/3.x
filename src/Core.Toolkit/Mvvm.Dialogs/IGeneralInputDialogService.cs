using System;
using System.Reactive;

namespace Core.Mvvm.Dialogs
{
    public interface IGeneralInputDialogService<in TInputData> : IGeneralInputOutputDialogService<TInputData, Unit>
    {
        //        IObservable<DialogEventArgs<Unit>> Show(TInputData inputData);
    }
}