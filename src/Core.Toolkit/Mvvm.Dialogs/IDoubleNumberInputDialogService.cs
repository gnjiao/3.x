using System;

namespace Core.Mvvm.Dialogs
{
    public interface IDoubleNumberInputDialogService//: IGeneralOutputDialogService<double>
    {
        IObservable<DialogEventArgs<double>> Show(double minValue, double maxValue,string titile=null);
    }
}