using System;

namespace Core.Mvvm.Dialogs
{
    public interface IGeneralInputOutputDialogService<in TInputData, TOutputData>
    {
        IObservable<DialogEventArgs<TOutputData>> Show(TInputData inputData);
    }

    public interface IGeneralInputOutputDialogService<TInputOutputData> : IGeneralInputOutputDialogService<
                                                                              TInputOutputData,
                                                                              TInputOutputData>
    {
    }
}