using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class AutoThresholdRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = image.AutoThreshold(Sigma);
            return region;
        }

        [Description("高斯平滑的直方图因子, 建议值: 0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0")]
        public double Sigma { get; set; } = 2.0;
    }
}