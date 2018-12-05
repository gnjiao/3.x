using System;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MedianImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage enhancedImage = image.MedianImage(
                MaskType.ToHalconString(),
                Radius,
                Margin.ToHalconString());

            return enhancedImage;
        }

        [DefaultValue(MedianMaskType.Circle)]
        [Browsable(true)]
        [Description("滤波形状的选择: 'circle', 'square'")]
        public MedianMaskType MaskType { get; set; } = MedianMaskType.Circle;

        [DefaultValue(1)]
        [Browsable(true)]
        [Description("滤波内核的半径, 建议值: 1, 2,3, 4, 5, 6, 7, 8, 9, 11, 15, 19, 25, 31, 39, 47, 59")]
        public int Radius { get; set; } = 1;

        [DefaultValue(MedianMargin.Mirrored)]
        [Browsable(true)]
        [Description("边界处理的形式, 建议值: 'mirrored', 'cyclic', 'continued', 0, 30, 60, 90, 120, 150, 180, 210, 240, 255")]
        public MedianMargin Margin { get; set; } = MedianMargin.Mirrored;
    }
}