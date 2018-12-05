using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Hdc.Mv.PropertyItem.Controls
{
    public class SaveDialogEditor : TypeEditor<FileDialogControl>
    {
        protected override FileDialogControl CreateEditor()
        {
            return new FileDialogControl(false, "Config Files|*.*");
        }

        protected override void SetValueDependencyProperty()
        {
            ValueProperty = FileDialogControl.FileNameProperty;
        }
    }
}
