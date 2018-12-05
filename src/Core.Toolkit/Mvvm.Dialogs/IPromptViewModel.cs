using System;

namespace Core.Mvvm.Dialogs
{
    public interface IPromptViewModel<TValue>
    {
        IObservable<DialogEventArgs<TValue>> ClosedEvent { get; }
    }
}