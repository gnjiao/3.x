using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Hdc.Mv.PropertyItem.Controls
{    
    public class TemplateMatchingBlockEditor : TypeEditor<ReadImageBlockControl>
    {
        protected override ReadImageBlockControl CreateEditor()
        {
            return new ReadImageBlockControl($"Config Files|*.*");
        }

        protected override void SetValueDependencyProperty()
        {
            ValueProperty = ReadImageBlockControl.FileNameProperty;
        }
    }
}
