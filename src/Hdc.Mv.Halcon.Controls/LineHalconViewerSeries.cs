using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class LineHalconViewerSeries : HalconViewerSeries
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
            "Row1", typeof(string), typeof(LineHalconViewerSeries));

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
            "Column1", typeof(string), typeof(LineHalconViewerSeries));

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
            "Row2", typeof(string), typeof(LineHalconViewerSeries));

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
            "Column2", typeof(string), typeof(LineHalconViewerSeries));

        #endregion                       
        private static double DoubleCheck(double value)
        {
            return double.IsInfinity(value) || double.IsNaN(value) ? 0 : value;
        }

        protected override HRegion GetDisplayRegion(object element)
        {
            var row1 = DoubleCheck((double)GetPropertyValue(element, Row1));
            var column1 = DoubleCheck((double)GetPropertyValue(element, Column1));
            var row2 = DoubleCheck((double)GetPropertyValue(element, Row2));
            var column2 = DoubleCheck((double)GetPropertyValue(element, Column2));

            var line = new HRegion();
            line.GenRegionLine(row1, column1, row2, column2);
            return line;
        }
    }
}