using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class Rectangle2HalconViewerSeries : HalconViewerSeries
    {
        #region Row
        /// <summary>
        /// Row
        /// </summary>
        public string Row
        {
            get => (string)GetValue(RowProperty);
            set => SetValue(RowProperty, value);
        }

        public static readonly DependencyProperty RowProperty = DependencyProperty.Register(
            "Row", typeof(string), typeof(Rectangle2HalconViewerSeries));

        #endregion

        #region Column
        /// <summary>
        /// Column
        /// </summary>
        public string Column
        {
            get => (string)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
            "Column", typeof(string), typeof(Rectangle2HalconViewerSeries));

        #endregion

        #region Phi
        /// <summary>
        /// Phi
        /// </summary>
        public string Phi
        {
            get => (string)GetValue(PhiProperty);
            set => SetValue(PhiProperty, value);
        }

        public static readonly DependencyProperty PhiProperty = DependencyProperty.Register(
            "Phi", typeof(string), typeof(Rectangle2HalconViewerSeries));

        #endregion

        #region Length1
        /// <summary>
        /// Length1
        /// </summary>
        public string Length1
        {
            get => (string)GetValue(Length1Property);
            set => SetValue(Length1Property, value);
        }

        public static readonly DependencyProperty Length1Property = DependencyProperty.Register(
            "Length1", typeof(string), typeof(Rectangle2HalconViewerSeries));

        #endregion

        #region Length2
        /// <summary>
        /// Length2
        /// </summary>
        public string Length2
        {
            get => (string)GetValue(Length2Property);
            set => SetValue(Length2Property, value);
        }

        public static readonly DependencyProperty Length2Property = DependencyProperty.Register(
            "Length2", typeof(string), typeof(Rectangle2HalconViewerSeries));

        #endregion
        private static double DoubleCheck(double value)
        {
            return double.IsInfinity(value) || double.IsNaN(value) ? 0 : value;
        }

        protected override HRegion GetDisplayRegion(object element)
        {
            var row = DoubleCheck((double)GetPropertyValue(element, Row));
            var column = DoubleCheck((double)GetPropertyValue(element, Column));
            var phi = DoubleCheck((double)GetPropertyValue(element, Phi));
            var length1 = DoubleCheck((double)GetPropertyValue(element, Length1));
            var length2 = DoubleCheck((double)GetPropertyValue(element, Length2));
            
            var rectangle2 = new HRegion();
            rectangle2.GenRectangle2(row, column, phi, length1, length2);
            var rectangle2Boundary = rectangle2.Boundary("inner");
            rectangle2.Dispose();
            return rectangle2Boundary;
        }
    }
}