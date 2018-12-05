using System;
using HalconDotNet;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GrayRangeRectImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            return image.GrayRangeRect(MaskHeight, MaskWidth);
        }

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("滤波内核的高度, 建议值: 3, 5, 7, 9, 11, 13, 15")]
        public int MaskHeight { get; set; } = 11;

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("滤波内核的宽度, 建议值: 3, 5, 7, 9, 11, 13, 15")]
        public int MaskWidth { get; set; } = 11;
    }
}