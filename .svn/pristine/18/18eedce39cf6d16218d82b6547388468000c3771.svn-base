using System;
using System.ComponentModel;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class BinomialImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            if (MaskWidth < 1 || MaskWidth > 37 || MaskHeight < 1 || MaskHeight > 37)
                throw new InvalidOperationException("BinomialFilter MaskWidth/MaskHeight <1 or >37");

            //            var swMeanImage = new NotifyStopwatch("MeanFilter.MeanImage");
            HImage enhancedImage = image.BinomialFilter(MaskWidth, MaskHeight);
            //            swMeanImage.Dispose();

            return enhancedImage;
        }

        [DefaultValue(5)]
        [Browsable(true)] //yx
        [Description("滤波内核宽度, 可选值为: 1, 3, 5, 7, 9, 11, 13, 15 ...")]
        public int MaskWidth { get; set; } = 5;

        [DefaultValue(5)]
        [Browsable(true)] //yx
        [Description("滤波内核高度, 可选值为: 1, 3, 5, 7, 9, 11, 13, 15 ...")]
        public int MaskHeight { get; set; } = 5;
    }
}