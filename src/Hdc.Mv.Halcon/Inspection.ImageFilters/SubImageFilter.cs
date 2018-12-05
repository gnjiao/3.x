using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SubImageFilter : ImageFilterBase
    {
        public IImageFilter SubtrahendImageFilter { get; set; }
        [Description("校正因子，建议值：0.0, 1.0, 2.0, 3.0, 4.0")]
        public double Mult { get; set; } = 1.0;

        [Description("校正值，建议值：0.0, 128.0, 256.0")]
        public double Add { get; set; } = 128.0;

        protected override HImage ProcessInner(HImage image)
        {
            var subtrahendImage = SubtrahendImageFilter.Process(image);

            HImage subImage = image.SubImage(subtrahendImage, Mult, Add);

            subtrahendImage.Dispose();

            return subImage;
        }
    }
}