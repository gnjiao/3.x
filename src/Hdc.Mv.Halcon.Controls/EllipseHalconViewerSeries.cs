using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class EllipseHalconViewerSeries : HalconViewerSeries
    {
        #region Row

        public string Row
        {
            get => (string)GetValue(RowProperty);
            set => SetValue(RowProperty, value);
        }

        public static readonly DependencyProperty RowProperty = DependencyProperty.Register(
            "Row", typeof(string), typeof(EllipseHalconViewerSeries));

        #endregion

        #region Column

        public string Column
        {
            get => (string)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
            "Column", typeof(string), typeof(EllipseHalconViewerSeries));

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
            "Phi", typeof(string), typeof(EllipseHalconViewerSeries));

        #endregion

        #region Radius1

        public string Radius1
        {
            get => (string)GetValue(Radius1Property);
            set => SetValue(Radius1Property, value);
        }

        public static readonly DependencyProperty Radius1Property = DependencyProperty.Register(
            "Radius1", typeof(string), typeof(EllipseHalconViewerSeries));

        #endregion

        #region Radius2

        public string Radius2
        {
            get => (string)GetValue(Radius2Property);
            set => SetValue(Radius2Property, value);
        }

        public static readonly DependencyProperty Radius2Property = DependencyProperty.Register(
            "Radius2", typeof(string), typeof(EllipseHalconViewerSeries));

        #endregion

        protected override HRegion GetDisplayRegion(object element)
        {
            var row = DoubleCheck((double)GetPropertyValue(element, Row));
            var column = DoubleCheck((double)GetPropertyValue(element, Column));
            var phi = DoubleCheck((double)GetPropertyValue(element, Phi));
            var radius1 = DoubleCheck((double)GetPropertyValue(element, Radius1));
            var radius2 = DoubleCheck((double)GetPropertyValue(element, Radius2));

            var ellipse = new HRegion();
            ellipse.GenEllipse(row, column, phi,radius1, radius2);
            var ellipseBoundary = ellipse.Boundary("inner");
            ellipse.Dispose();
            return ellipseBoundary;
        }
        private static double DoubleCheck(double value)
        {
            return double.IsInfinity(value) || double.IsNaN(value) ? 0 : value;
        }
    }
}
