using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class Rectangle1HalconViewerSeries : HalconViewerSeries
    {
        #region Row1
        /// <summary>
        /// Row
        /// </summary>
        public string Row1
        {
            get => (string)GetValue(Row1Property);
            set => SetValue(Row1Property, value);
        }

        public static readonly DependencyProperty Row1Property = DependencyProperty.Register(
            "Row1", typeof(string), typeof(Rectangle1HalconViewerSeries));

        #endregion

        #region Column1
        /// <summary>
        /// Row
        /// </summary>
        public string Column1
        {
            get => (string)GetValue(Column1Property);
            set => SetValue(Column1Property, value);
        }

        public static readonly DependencyProperty Column1Property = DependencyProperty.Register(
            "Column1", typeof(string), typeof(Rectangle1HalconViewerSeries));

        #endregion

        #region Row2
        /// <summary>
        /// Row
        /// </summary>
        public string Row2
        {
            get => (string)GetValue(Row2Property);
            set => SetValue(Row2Property, value);
        }

        public static readonly DependencyProperty Row2Property = DependencyProperty.Register(
            "Row2", typeof(string), typeof(Rectangle1HalconViewerSeries));

        #endregion

        #region Column2
        /// <summary>
        /// Row
        /// </summary>
        public string Column2
        {
            get => (string)GetValue(Column2Property);
            set => SetValue(Column2Property, value);
        }

        public static readonly DependencyProperty Column2Property = DependencyProperty.Register(
            "Column2", typeof(string), typeof(Rectangle1HalconViewerSeries));

        #endregion                       
        private static double DoubleCheck(double value)
        {
            return double.IsInfinity(value) || double.IsNaN(value) ? 0 : value;
        }

        protected override HRegion GetDisplayRegion(object element)
        {
            var row1 = (double)GetPropertyValue(element, Row1);
            var column1 = (double)GetPropertyValue(element, Column1);
            var row2 = (double)GetPropertyValue(element, Row2);
            var column2 = (double)GetPropertyValue(element, Column2);

            row1 = DoubleCheck(row1);
            column1 = DoubleCheck(column1);
            row2 = DoubleCheck(row2);
            column2 = DoubleCheck(column2);

            var rectangle1 = new HRegion();
            rectangle1.GenRectangle1(row1, column1, row2, column2);
            var rectangle1Boundary = rectangle1.Boundary("inner");
            rectangle1.Dispose();
            return rectangle1Boundary;
        }
    }
}
