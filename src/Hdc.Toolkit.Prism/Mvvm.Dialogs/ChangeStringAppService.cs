using System;
using Core.Mvvm.Dialogs;
using Microsoft.Practices.Unity;

namespace Hdc.Mvvm.Dialogs
{
    public class ChangeStringAppService : IChangeStringAppService
    {
        [Dependency]
        public IStringInputDialogService StringInputDialogService { get; set; }


        public void ChangeString(string title, Func<string> getDefaultString, Action<string> setEditedString)
        {
            StringInputDialogService
                .Show(title, getDefaultString())
                .Subscribe(
                    args =>
                        {
                            if (args.IsCanceled)
                                return;

                            setEditedString(args.Data);
                        });
        }
    }
}