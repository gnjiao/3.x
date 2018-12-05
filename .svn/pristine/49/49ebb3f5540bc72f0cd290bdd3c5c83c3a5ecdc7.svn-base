using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class HighpassImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var enhancedImage = image.HighpassImage(Width, Height);

            return enhancedImage;
        }
        [Description("滤波内核的宽度, 建议值: 3, 5, 7, 9, 11, 13, 17, 21, 29, 41, 51, 73, 101")]
        public int Width { get; set; } = 9;

        [Description("滤波内核的高度, 建议值: 3, 5, 7, 9, 11, 13, 17, 21, 29, 41, 51, 73, 101")]
        public int Height { get; set; } = 9;
    }
}