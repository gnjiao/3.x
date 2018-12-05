using System;
using HalconDotNet;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class AdjustMeanImageFilter: ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var domain = image.GetDomain();
            double deviation;
            var mean = image.Intensity(domain, out deviation);
            var diff = TargetGrayMean - mean;
            var scaleImage = image.ScaleImage(1, diff);
            return scaleImage;
        }

        [DefaultValue(128)]
        [Browsable(true)] //yx
        [Description("目标灰度均值")]
        public int TargetGrayMean { get; set; } = 128;
    }
}