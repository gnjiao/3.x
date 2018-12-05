using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class IntensityAndScale2ImageFilter : ImageFilterBase, IImageFilter
    {
        protected override HImage ProcessInner(HImage image)
        {
            var mean = image.GetMeanGrayValueUsingIntensityOfDomain();
            var realadd = MakeImageDarker ? -Mult * (mean + Add) : Mult * (mean + Add);

            var scaledImage = image.ScaleImage(Mult, realadd);
            return scaledImage;

        }

        public double Mult { get; set; }

        public double Add { get; set; }

        public bool MakeImageDarker { get; set; }

    }
}
