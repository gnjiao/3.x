using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace Hdc.Mv.PropertyItem.Controls
{
    public class FileDialogControl : Control
    {
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(FileDialogControl),
                new PropertyMetadata(new PropertyChangedCallback(OnFileNameChanged)));

        public static readonly DependencyProperty ShowDialogCommandProperty =
            DependencyProperty.Register("ShowDialogCommand", typeof(ICommand), typeof(FileDialogControl));

        public static readonly RoutedEvent FileNameChangedEvent =
            EventManager.RegisterRoutedEvent("FileNameChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>), typeof(FileDialogControl));

        public ICommand ShowDialogCommand
        {
            get => (ICommand)GetValue(ShowDialogCommandProperty);
            set => SetValue(ShowDialogCommandProperty, value);
        }

        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        public event RoutedPropertyChangedEventHandler<string> FileNameChanged
        {
            add => AddHandler(FileNameChangedEvent, value);
            remove => RemoveHandler(FileNameChangedEvent, value);
        }

        static FileDialogControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileDialogControl), new FrameworkPropertyMetadata(typeof(FileDialogControl)));
        }

        public FileDialogControl()
        {

        }
        public FileDialogControl(bool dialogType, string filter)
        {            
            ShowDialogCommand = new DelegateCommand(() => { ShowDialog(dialogType, filter); });
        }
        
        private void ShowDialog(bool dialogType, string filter)
        {
            FileDialog dialog;

            if (dialogType)
            {
                dialog = new OpenFileDialog()
                {
                    Filter = filter
                };
            }
            else
            {
                dialog = new SaveFileDialog()
                {
                    Filter = filter
                };
            }

            if (dialog.ShowDialog(null) != true)
                return;

            FileName = dialog.FileName;
        }

        private void OnFileNameChanged(string oldValue, string newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<string>(oldValue, newValue, FileNameChangedEvent);
            RaiseEvent(args);
        }

        private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var filename = (FileDialogControl)d;
            filename.OnFileNameChanged((string)e.OldValue, (string)e.NewValue);
        }

    }
}
