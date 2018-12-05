using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace Hdc.Mv.PropertyItem.Controls
{
    /// <inheritdoc />
    /// <![CDATA['MyNamespace' is an undeclared prefix. Line 28, position 7.]]>
    public class ReadImageBlockControl : Control
    {
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(ReadImageBlockControl),
                new PropertyMetadata(new PropertyChangedCallback(OnFileNameChanged)));

        public static readonly DependencyProperty ReadImageCommandProperty =
            DependencyProperty.Register("ReadImageCommand", typeof(ICommand), typeof(ReadImageBlockControl));

        public static readonly RoutedEvent FileNameChangedEvent =
            EventManager.RegisterRoutedEvent("FileNameChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>), typeof(ReadImageBlockControl));
        
        public ICommand ReadImageCommand
        {
            get => (ICommand)GetValue(ReadImageCommandProperty);
            set => SetValue(ReadImageCommandProperty, value);
        }

        public string FileName
        {
            get => (string) GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        public event RoutedPropertyChangedEventHandler<string> FileNameChanged
        {
            add => AddHandler(FileNameChangedEvent, value);
            remove => RemoveHandler(FileNameChangedEvent, value);
        }

        static ReadImageBlockControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReadImageBlockControl), new FrameworkPropertyMetadata(typeof(ReadImageBlockControl)));
        }

        public ReadImageBlockControl()
        {
            FileName = "";
            ReadImageCommand = new DelegateCommand(ReadImage);
        }

        public ReadImageBlockControl(string filter)
        {
            FileName = "";
            ReadImageCommand = new DelegateCommand(() =>
            {
                var dialog = new OpenFileDialog
                {
                    Filter = filter
                };

                if (dialog.ShowDialog(null) != true)
                    return;

                FileName = dialog.FileName;
            });
        }

        private void ReadImage()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "TIFF|*.tiff|TIF|*.tif|BMP|*.bmp|JPG|*.jpg|Image Files|*.*"
            };

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
            var filename = (ReadImageBlockControl)d;
            filename.OnFileNameChanged((string)e.OldValue, (string)e.NewValue);
        }

    }
}
