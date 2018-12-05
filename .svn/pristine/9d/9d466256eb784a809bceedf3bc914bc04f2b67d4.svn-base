using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Platform.Main.Annotations;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="IPropertyBrowserView" />
    /// <summary>
    /// Interaction logic for PropertyBrowserView.xaml
    /// </summary>
    public sealed partial class PropertyBrowserView : IPropertyBrowserView, INotifyPropertyChanged
    {
        public PropertyBrowserView()
        {
            InitializeComponent();

            BlockPropertyGrid.PropertyValueChanged += BlockPropertyGrid_PropertyValueChanged;            
        }
        
        private void BlockPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (!Equals(e.NewValue,e.OldValue))
                MainWindow.Instance.SelectedPropertyGridItem = BlockPropertyGrid.SelectedPropertyItem;                  
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}
