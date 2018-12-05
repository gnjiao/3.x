using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class HysteresisThresholdRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = image.HysteresisThreshold(Low, High, MaxLength);
            return region;
        }
        [Description("灰度值的下阀值，建议值： 5, 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100")]
        public int Low { get; set; } = 30;

        [Description("灰度值的上阀值，建议值：5, 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130")]
        public int High { get; set; } = 60;

        [Description("到达安全路径潜在点的最大长度，建议值：1, 2, 3, 5, 7, 10, 12, 14, 17, 20, 25, 30, 35, 40, 50")]
        public int MaxLength { get; set; } = 10;
    }
}