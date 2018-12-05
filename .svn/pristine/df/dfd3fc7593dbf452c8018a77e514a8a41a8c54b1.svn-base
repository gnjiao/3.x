using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class CircleRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var rect = new HRegion();
            rect.GenCircle(Y, X, Radius);
            return rect;
        }
        [Description("圆心X坐标")]
        public double X { get; set; }

        [Description("圆心Y坐标")]
        public double Y { get; set; }

        [Description("圆的半径")]
        public double Radius { get; set; }
    }
}