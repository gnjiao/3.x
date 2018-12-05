using System;
using HalconDotNet;
using System.ComponentModel;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ScaleImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var scaledImage = image.ScaleImage(Mult, Add);

            return scaledImage;
        }

        [DefaultValue(0.01)]
        [Browsable(true)]
        [Description("比例因子,建议值: 0.001, 0.003, 0.005, 0.008, 0.01, 0.02, 0.03, 0.05, 0.08, 0.1, 0.5, 1.0")]
        public double Mult { get; set; } = 0.01;

        [DefaultValue(0)]
        [Browsable(true)]
        [Description("偏移量,建议值: 0, 10, 50, 100, 200, 500")]
        public double Add { get; set; } = 0.0;

    }
}