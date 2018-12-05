using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SubImage2Filter : ImageFilterBase
    {
        public IImageFilter MinuendImageFilter { get; set; }

        public IImageFilter SubtrahendImageFilter { get; set; }

        [Description("校正因子，建议值：0.0, 1.0, 2.0, 3.0, 4.0")]
        public double Mult { get; set; } = 1.0;

        [Description("校正值，建议值：0.0, 128.0, 256.0")]
        public double Add { get; set; } = 128.0;

        protected override HImage ProcessInner(HImage image)
        {
            var minuendImage = MinuendImageFilter.Process(image);
            var subtrahendImage = SubtrahendImageFilter.Process(image);

            HImage subImage = minuendImage.SubImage(subtrahendImage, Mult, Add);

            minuendImage.Dispose();
            subtrahendImage.Dispose();

            return subImage;
        }
    }
}