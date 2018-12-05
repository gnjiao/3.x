using System;
using HalconDotNet;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ExpandDomainGrayImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage enhancedImage = image.ExpandDomainGray(ExpansionRange);

            return enhancedImage;
        }

        [DefaultValue(2)]
        [Browsable(true)] //yx
        [Description("灰度值展开的半径, 以像素为单位进行度量, " +
                     "建议值: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 16")]
        public int ExpansionRange { get; set; } = 2;
    }
}