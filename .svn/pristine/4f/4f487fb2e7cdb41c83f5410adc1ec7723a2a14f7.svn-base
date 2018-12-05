using System;
using System.Reactive;
using Core.Mvvm.Dialogs;

namespace Hdc.Mvvm.Dialogs
{
    public abstract class GeneralInputDialogService<TInputData> :
        GeneralInputOutputDialogService<TInputData, Unit>,
        IGeneralInputDialogService<TInputData>
    {
        protected override Unit GetOutput()
        {
            return new Unit();
        }
    }
}