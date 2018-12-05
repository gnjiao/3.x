using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ThresholdRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = image.Threshold(MinGray, MaxGray);
            return region;
        }

        [Description("灰度值的下阀值,建议值： 0.0, 10.0, 30.0, 64.0, 128.0, 200.0, 220.0, 255.0")]
        public double MinGray { get; set; }

        [Description("灰度值的上阀值,建议值： 0.0, 10.0, 30.0, 64.0, 128.0, 200.0, 220.0, 255.0")]

        public double MaxGray { get; set; } = 255;
    }
}