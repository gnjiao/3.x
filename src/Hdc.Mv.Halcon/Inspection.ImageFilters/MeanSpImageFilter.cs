using System;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MeanSpImageFilter : IImageFilter
    {
        public HImage Process(HImage image)
        {
            var maskWidth = MaskWidth;
            var maskHeight = MaskHeight;

            HImage enhancedImage = image.MeanSp(maskHeight, maskWidth, MinThresh, MaxThresh);

            return enhancedImage;
        }

        [DefaultValue(3)]
        [Browsable(true)]
        [Description("滤波内核的宽度，建议值：3, 5, 7, 9, 11")]
        public int MaskWidth { get; set; } = 3;//yx

        [DefaultValue(3)]
        [Browsable(true)]
        [Description("滤波的高度，建议值：3, 5, 7, 9, 11")]
        public int MaskHeight { get; set; } = 3;//yx

        [DefaultValue(1)]
        [Browsable(true)]
        [Description("最小灰度值，建议值：1, 5, 7, 9, 11, 15, 23, 31, 43, 61, 101")]
        public int MinThresh { get; set; } = 1;//yx

        [DefaultValue(254)]
        [Browsable(true)]
        [Description("最大灰度值，建议值：5, 7, 9, 11, 15, 23, 31, 43, 61, 101, 200, 230, 250, 254")]
        public int MaxThresh { get; set; } = 254;//yx
    }
}