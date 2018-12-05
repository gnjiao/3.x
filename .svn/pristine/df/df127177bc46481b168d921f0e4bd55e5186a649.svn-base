using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class PaintRegionUsingHysteresisThresholdImageFilter: ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var inputImage = InvertPreprocess ? image.InvertImage() : image;

            var region = image.HysteresisThreshold(HysteresisLow, HysteresisHigh, HysteresisMaxLength);
            var paintedImage = inputImage.PaintRegion(region, (double)PaintRegionGrayValue, "fill");
            return paintedImage;
        }

        public bool InvertPreprocess { get; set; }
        public int PaintRegionGrayValue { get; set; } = 255;
        public int HysteresisLow { get; set; } = 30;
        public int HysteresisHigh { get; set; } = 60;
        public int HysteresisMaxLength { get; set; } = 10;
    }
}