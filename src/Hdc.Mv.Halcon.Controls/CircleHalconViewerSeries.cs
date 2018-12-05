using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class CircleHalconViewerSeries : HalconViewerSeries
    {
        #region Row

        public string Row
        {
            get => (string)GetValue(RowProperty);
            set => SetValue(RowProperty, value);
        }

        public static readonly DependencyProperty RowProperty = DependencyProperty.Register(
            "Row", typeof(string), typeof(CircleHalconViewerSeries));

        #endregion

        #region Column

        public string Column
        {
            get => (string)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
            "Column", typeof(string), typeof(CircleHalconViewerSeries));

        #endregion

        #region Radius

        public string Radius
        {
            get => (string)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius", typeof(string), typeof(CircleHalconViewerSeries));

        #endregion
        protected override HRegion GetDisplayRegion(object element)
        {
            var row = DoubleCheck((double)GetPropertyValue(element, Row));
            var col = DoubleCheck((double)GetPropertyValue(element, Column));
            var radius = DoubleCheck((double)GetPropertyValue(element, Radius));
            
            var circle = new HRegion();
            circle.GenCircle(row, col, radius);
            var circleBoundary = circle.Boundary("inner");
            circle.Dispose();
            return circleBoundary;
        }
        private static double DoubleCheck(double value)
        {
            return double.IsInfinity(value) || double.IsNaN(value) ? 0 : value;
        }

        
    }
}