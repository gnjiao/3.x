using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class BinaryThresholdDualRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            int usedThreshold;
            var region1 = image.BinaryThreshold("max_separability", LightDark1.ToHalconString(), out usedThreshold);
            var reducedImage = image.ReduceDomain(region1);

            int usedThreshold2;
            var region2 = reducedImage.BinaryThreshold("max_separability", LightDark2.ToHalconString(), out usedThreshold2);

            reducedImage.Dispose();

            return region2;
        }

        [Description("��ȡǰ�����߱���,��ѡֵ��'Light', 'Dark', 'Equal', 'NotEqual' ")]
        public LightDark LightDark1 { get; set; }

        [Description("��ȡǰ�����߱���,��ѡֵ��'Light', 'Dark', 'Equal', 'NotEqual'")]
        public LightDark LightDark2 { get; set; }
    }
}