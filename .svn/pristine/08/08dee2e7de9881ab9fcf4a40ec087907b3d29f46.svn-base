using System;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GrayClosingShapeImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var domainWidth = image.GetDomain().GetWidth();
            var domainHeight = image.GetDomain().GetHeight();

            var finalHeight = Math.Abs(MaskHeight) < 0.000001 ? domainHeight : MaskHeight;
            var finalWidth = Math.Abs(MaskWidth) < 0.000001 ? domainWidth : MaskWidth;
            return image.GrayClosingShape(finalHeight, finalWidth, MaskShape.ToHalconString());
        }

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("滤波内核的高度, 建议值: 3, 5, 7, 9, 11, 13, 15")]
        public double MaskHeight { get; set; } = 11;

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("滤波内核的宽度, 建议值: 3, 5, 7, 9, 11, 13, 15")]
        public double MaskWidth { get; set; } = 11;

        [DefaultValue("octagon")]
        [Browsable(true)] //yx
        [Description("滤波内核的形状,可选值有: 'octagon', 'rectangle', 'rhombus'")]
        public MaskShape MaskShape { get; set; } = MaskShape.Octagon;
    }
}