using System.Windows;
using System.Windows.Controls;
using Hdc.Mv.Inspection;

namespace Hdc.Controls
{
    /// <summary>
    /// Interaction logic for FrameStateIndicator.xaml
    /// </summary>
    public partial class FrameStateIndicator : UserControl
    {
        public FrameStateIndicator()
        {
            InitializeComponent();
        }

        #region FrameState

        public FrameState FrameState
        {
            get { return (FrameState)GetValue(FrameStateProperty); }
            set { SetValue(FrameStateProperty, value); }
        }

        public static readonly DependencyProperty FrameStateProperty = DependencyProperty.Register(
            "FrameState", typeof(FrameState), typeof(FrameStateIndicator));

        #endregion
    }
}
