using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class XldHalconViewerSeries: HalconViewerSeries
    {
        protected override HRegion GetDisplayRegion(object element)
        {
            return null;
        }

        #region XldPath

        public string XldPath
        {
            get { return (string)GetValue(XldPathProperty); }
            set { SetValue(XldPathProperty, value); }
        }

        public static readonly DependencyProperty XldPathProperty = DependencyProperty.Register(
            "XldPath", typeof(string), typeof(XldHalconViewerSeries));

        #endregion

        protected override HXLD GetDisplayXld(object element)
        {
            if (string.IsNullOrEmpty(XldPath))
                return null;

            var xld = (HXLD)GetPropertyValue(element, XldPath);

            return xld;
        }
    }
}