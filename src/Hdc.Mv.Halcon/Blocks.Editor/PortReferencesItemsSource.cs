
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Hdc.Mv.Halcon
{
    public abstract class PortReferencesItemsSource : IItemsSource
    {
        public abstract ItemCollection GetValues();        
    }

    public class DropDownItem : TypeConverter
    {

    }
}
