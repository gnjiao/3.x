using System;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GrayDilationRectImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var domainWidth = image.GetDomain().GetWidth();
            var domainHeight = image.GetDomain().GetHeight();

            var finalHeight = MaskHeight == 0 ? domainHeight : MaskHeight;
            var finalWidth = MaskWidth == 0 ? domainWidth : MaskWidth;
            return image.GrayDilationRect(finalHeight, finalWidth);
        }

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("�˲��ں˵ĸ߶�, ����ֵ: 3, 5, 7, 9, 11, 13, 15")]
        public int MaskHeight { get; set; } = 11;

        [DefaultValue(11)]
        [Browsable(true)] //yx
        [Description("�˲��ں˵Ŀ��, ����ֵ: 3, 5, 7, 9, 11, 13, 15")]
        public int MaskWidth { get; set; } = 11;
    }
}