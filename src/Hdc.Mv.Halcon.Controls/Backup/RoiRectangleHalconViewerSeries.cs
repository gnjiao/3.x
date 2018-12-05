using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using HalconDotNet;

namespace Hdc.Controls
{
    public class RoiRectangleHalconViewerSeries: HalconViewerSeries
    {
        protected override HRegion GetDisplayRegion(object element)
        {
            var startX = (double)GetPropertyValue(element, StartXPath);
            var startY = (double)GetPropertyValue(element, StartYPath);
            var endX = (double)GetPropertyValue(element, EndXPath);
            var endY = (double)GetPropertyValue(element, EndYPath);
            var halfWidth = (double)GetPropertyValue(element, HalfWidthPath);

            var centerX = (startX + endX) / 2.0;
            var centerY = (startY + endY) / 2.0;

            var startVector = new Vector(startX, startY);
            var endVector = new Vector(endX, endY);
            Vector linkVector = (startVector - endVector);
            var length = linkVector.Length;
            var angle = Vector.AngleBetween(linkVector, new Vector(0, 100));
            var anglePhi = angle / 360.0 * 2 * Math.PI;

            var rect2 = new HRegion();
            rect2.GenRectangle2(centerY, centerX, anglePhi, halfWidth, length / 2.0);

            var inner = rect2.Boundary("inner");

            rect2.Dispose();
            return inner;
        }

        #region StartXPath

        public string StartXPath
        {
            get { return (string) GetValue(StartXPathProperty); }
            set { SetValue(StartXPathProperty, value); }
        }

        public static readonly DependencyProperty StartXPathProperty = DependencyProperty.Register(
            "StartXPath", typeof (string), typeof (RoiRectangleHalconViewerSeries));

        #endregion

        #region StartYPath

        public string StartYPath
        {
            get { return (string) GetValue(StartYPathProperty); }
            set { SetValue(StartYPathProperty, value); }
        }

        public static readonly DependencyProperty StartYPathProperty = DependencyProperty.Register(
            "StartYPath", typeof (string), typeof (RoiRectangleHalconViewerSeries));

        #endregion

        #region EndXPath

        public string EndXPath
        {
            get { return (string) GetValue(EndXPathProperty); }
            set { SetValue(EndXPathProperty, value); }
        }

        public static readonly DependencyProperty EndXPathProperty = DependencyProperty.Register(
            "EndXPath", typeof (string), typeof (RoiRectangleHalconViewerSeries));

        #endregion

        #region EndYPath

        public string EndYPath
        {
            get { return (string) GetValue(EndYPathProperty); }
            set { SetValue(EndYPathProperty, value); }
        }

        public static readonly DependencyProperty EndYPathProperty = DependencyProperty.Register(
            "EndYPath", typeof (string), typeof (RoiRectangleHalconViewerSeries));

        #endregion

        #region HalfWidthPath

        public string HalfWidthPath
        {
            get { return (string) GetValue(HalfWidthPathProperty); }
            set { SetValue(HalfWidthPathProperty, value); }
        }

        public static readonly DependencyProperty HalfWidthPathProperty = DependencyProperty.Register(
            "HalfWidthPath", typeof (string), typeof (RoiRectangleHalconViewerSeries));

        #endregion
    }
}