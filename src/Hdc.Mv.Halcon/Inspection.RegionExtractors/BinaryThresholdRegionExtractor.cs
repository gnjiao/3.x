using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class BinaryThresholdRegionExtractor : RegionExtractorBase,IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            int usedThreshold;
            var region = image.BinaryThreshold("max_separability", LightDark.ToHalconString(), out usedThreshold);
            return region;
        }

        [Description("提取前景或者背景,可选值：'Light', 'Dark', 'Equal', 'NotEqual'")]
        public LightDark LightDark { get; set; } = LightDark.Dark;
        public string Name { get; set; }
    }
}