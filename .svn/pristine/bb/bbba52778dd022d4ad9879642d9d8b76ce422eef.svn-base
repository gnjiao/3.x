using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MeanAndSubImageFilter : ImageFilterBase, IImageFilter
    {
        protected override HImage ProcessInner(HImage image)
        {
            SubImageFilter subImageFilter = new SubImageFilter()
            {
                Mult = Mult,
                Add = Add,
                SubtrahendImageFilter = new MeanImageFilter(MaskWidth, MaskHeight)
            };
            var subImage = subImageFilter.Process(image);
            return subImage;
        }

        public int MaskWidth { get; set; }

        public int MaskHeight { get; set; }

        public double Mult { get; set; } = 1;

        public double Add { get; set; } = 128;
    }
}