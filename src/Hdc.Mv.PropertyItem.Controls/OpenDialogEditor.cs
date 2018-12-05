using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Hdc.Mv.PropertyItem.Controls
{
    public class OpenDialogEditor : TypeEditor<FileDialogControl>
    {
        protected override FileDialogControl CreateEditor()
        {
            return new FileDialogControl(true, "Config Files|*.*");
        }

        protected override void SetValueDependencyProperty()
        {
            ValueProperty = FileDialogControl.FileNameProperty;
        }
    }    
}
