using System;
using HalconDotNet;
using System.ComponentModel;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MedianRectImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage enhancedImage = image.MedianRect(
                MaskWidth, MaskHeight);

            return enhancedImage;
        }

        [DefaultValue(15)]
        [Browsable(true)]
        [Description("滤波内核的宽度, " +
                     "建议值: 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 31, 49, 51, 61, 71, 81, 91, 101")]
        public int MaskWidth { get; set; } = 15;

        [DefaultValue(15)]
        [Browsable(true)]
        [Description("滤波内核的高度, " +
                     "建议值: 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 31, 49, 51, 61, 71, 81, 91, 101")]
        public int MaskHeight { get; set; } = 15;
    }
}