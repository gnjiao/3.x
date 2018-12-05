using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class RegionHalconViewerSeries : HalconViewerSeries
    {
        protected override HRegion GetDisplayRegion(object element)
        {
            var region = (HRegion)GetPropertyValue(element, RegionPath);

            return region;
        }

        #region RegionPath

        public string RegionPath
        {
            get { return (string) GetValue(RegionPathProperty); }
            set { SetValue(RegionPathProperty, value); }
        }

        public static readonly DependencyProperty RegionPathProperty = DependencyProperty.Register(
            "RegionPath", typeof (string), typeof (RegionHalconViewerSeries));

        #endregion
    }
}