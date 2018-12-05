using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Hdc.Mv.PropertyItem.Controls
{
    /// <inheritdoc cref="ITypeEditor" />
    /// <summary>
    /// Interaction logic for RoiControlEditor.xaml
    /// </summary>
    public partial class CircleControlEditor : UserControl, ITypeEditor
    {
        public CircleControlEditor()
        {
            InitializeComponent();            
            
            RadioButtonCircle.AddHandler(RadioButton.MouseLeftButtonDownEvent, new RoutedEventHandler(ButtonRoutedEventHandler), true);
        }

        public void ButtonRoutedEventHandler(object sender, RoutedEventArgs e)
        {
            if (((RadioButton) sender).IsChecked != true) return;
            var selectedItem = ((RadioButton) sender).Content.ToString();            
            var dataObject = new DataObject(typeof(string), selectedItem);
            DragDrop.DoDragDrop((RadioButton)sender, dataObject, DragDropEffects.Copy);
        }

        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", typeof(string), typeof(RoiControlEditor),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            Binding binding = new Binding("Value");
            binding.Source = propertyItem;
            binding.Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(this, RoiControlEditor.ValueProperty, binding);
            return this;
        }

        private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var selectedItem = ((RadioButton)sender).Content.ToString();
            Value = selectedItem;
        }
    }
}
