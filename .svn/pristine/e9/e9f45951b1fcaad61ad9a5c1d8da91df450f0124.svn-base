using System;
using System.Threading;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GrayOpeningRectImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var domainWidth = image.GetDomain().GetWidth();
            var domainHeight = image.GetDomain().GetHeight();

            var finalHeight = MaskHeight == 0 ? domainHeight : MaskHeight;
            var finalWidth = MaskWidth == 0 ? domainWidth : MaskWidth;

//            var swGrayOpeningRect = new NotifyStopwatch("GrayOpeningRectFilter.GrayOpeningRect");
            var hImage = image.GrayOpeningRect(finalHeight, finalWidth);
//            swGrayOpeningRect.Dispose();
            return hImage;
        }

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("滤波内核的高度, 建议值: 3, 5, 7, 9, 11, 13, 15")]
        public int MaskHeight { get; set; } = 11;

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("滤波内核的高度, 建议值: 3, 5, 7, 9, 11, 13, 15")]
        public int MaskWidth { get; set; } = 11;
    }
}