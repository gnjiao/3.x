using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("PreprocessImageFilter")]
    public class IntensityAndScaleImageFilter : ImageFilterBase, IImageFilter
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage inputImage = image.CopyImage();
            if (PreprocessImageFilter != null)
            {
                var hImage = PreprocessImageFilter.Process(image);
                inputImage.Dispose();
                inputImage = hImage;
            }

            var mean = inputImage.GetMeanGrayValueUsingIntensityOfDomain();
            var differ = TargetMeanGray - mean;
            var scaleImage = inputImage.ScaleImage(1.0, differ);

            inputImage.Dispose();

            return scaleImage;
        }

        public int TargetMeanGray  { get; set; } = 128;

        public IImageFilter PreprocessImageFilter { get; set; }
    }
}