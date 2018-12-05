using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class FindHoleHalfCircleRegionExtractor : IRegionExtractor
    {
        public HRegion Extract(HImage image)
        {
            var foundRegion = image.FindHoleHalfCircleRegion(LeftOrRight, HasPre,
                HoughCircleThresholdGrayMin, HoughCircleThresholdGrayMax, HoughCircleSelectAreaMin,
                HoughCircleSelectAreaMax
                , HoughCircleRadius, HoughCirclePercent, MaxLineWidth);
            return foundRegion;
        }

        [Description("该值的一般区间: 0 ≤ LeftOrRight ≤ 1")]
        public bool LeftOrRight { get; set; }

        [Description("该值的一般区间: 0 ≤ HasPre ≤ 1")]
        public bool HasPre { get; set; }
        public int HoughCircleThresholdGrayMin { get; set; }
        public int HoughCircleThresholdGrayMax { get; set; }
        public int HoughCircleSelectAreaMin { get; set; }
        public int HoughCircleSelectAreaMax { get; set; }
        public int HoughCircleRadius { get; set; }
        public int HoughCirclePercent { get; set; }
        public double MaxLineWidth { get; set; }
    }
}