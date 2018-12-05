using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Platform.Main.Util
{
    public class FullScreenEnabledWindow : Window
    {
        public static readonly DependencyProperty FullScreenProperty =
            DependencyProperty.Register("FullScreen", typeof(bool), typeof(FullScreenEnabledWindow));

        public bool FullScreen
        {
            get { return (bool)GetValue(FullScreenProperty); }
            set { SetValue(FullScreenProperty, value); }
        }

        System.Windows.WindowState previousWindowState = WindowState.Maximized;
        double oldLeft, oldTop, oldWidth, oldHeight;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == FullScreenProperty)
            {
                if ((bool)e.NewValue)
                {
                    // enable fullscreen mode
                    // remember previous window state
                    if (this.WindowState == WindowState.Normal || this.WindowState == WindowState.Maximized)
                        previousWindowState = this.WindowState;
                    oldLeft = this.Left;
                    oldTop = this.Top;
                    oldWidth = this.Width;
                    oldHeight = this.Height;

                    var interop = new WindowInteropHelper(this);
                    interop.EnsureHandle();
                    Screen screen = Screen.FromHandle(interop.Handle);

                    Rect bounds = screen.Bounds.ToWpf().TransformFromDevice(this);

                    ResizeMode = ResizeMode.NoResize;
                    Left = bounds.Left;
                    Top = bounds.Top;
                    Width = bounds.Width;
                    Height = bounds.Height;
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.None;

                }
                else
                {
                    ClearValue(WindowStyleProperty);
                    ClearValue(ResizeModeProperty);
                    ClearValue(MaxWidthProperty);
                    ClearValue(MaxHeightProperty);
                    WindowState = previousWindowState;

                    Left = oldLeft;
                    Top = oldTop;
                    Width = oldWidth;
                    Height = oldHeight;
                }
            }
        }
    }
}
