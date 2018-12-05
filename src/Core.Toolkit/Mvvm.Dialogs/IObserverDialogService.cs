using System;
using System.Collections.Generic;

namespace Core.Mvvm.Dialogs
{
    public interface IObserverDialogService<in TData>
    {
        IObserver<TData> Show(TData data);
    }
}