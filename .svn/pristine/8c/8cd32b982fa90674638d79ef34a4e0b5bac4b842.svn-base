using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class RegionToBinImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var width = image.GetWidth();
            var height = image.GetHeight();
            var region = RegionExtractor.Extract(image);
            if (region.Area < 0.01)
            {
                return image;
            }
            var image2 = region.RegionToBin(ForegroundGray, BackgroundGray, width, height);
            return image2;
        }

        public IRegionExtractor RegionExtractor { get; set; }

        [Description("显示区域的灰度值，建议值： 0, 1, 50, 100, 128, 150, 200, 254, 255")]
        public int ForegroundGray { get; set; } = 255;

        [Description("显示背景的灰度值，建议值： 0, 1, 50, 100, 128, 150, 200, 254, 255")]
        public int BackgroundGray { get; set; } = 0;

        [Description("要生成图像的宽度，建议值：256, 512, 1024")]
        public int width { get; set; } = 512;
        [Description("要生成图像的高度，建议值：256, 512, 1024")]
        public int height { get; set; } = 512;
    }
}