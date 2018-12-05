using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Hdc.Mv.Halcon;
using Platform.Main.Views;
using Xceed.Wpf.AvalonDock.Layout;

namespace Platform.Main
{
    using Core;

    public class ExitCommand : SimpleCommand
    {
        public override void Execute(object parameter)
        {
            var mainWindow = parameter as MainWindow;

            mainWindow?.Close();            
        }
    }

    public class NewCommand : SimpleCommand
    {
        public override void Execute(object parameter)
        {
            var mainWindow = parameter as MainWindow;

            mainWindow?.AddTest1();
        }
    }

    public class RunCommand : SimpleCommand
    {
        public override void Execute(object parameter)
        {
            var mainWindow = parameter as MainWindow;

            mainWindow?.RunBlockSchema();

        }
    }
}
