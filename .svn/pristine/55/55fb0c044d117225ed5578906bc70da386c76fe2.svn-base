using System;
using System.Windows.Markup;
using HalconDotNet;
using System.ComponentModel;
using Hdc.Mv.Halcon;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class PaintRegionImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var region = RegionExtractor.Extract(image);
            if (region.Area <= 0.01)
            {
                return image;
            }
            var paintedImage = image.PaintRegion(region, (double) GrayValue, Type.ToString());
            return paintedImage;
        }
        [Description("期望区域的灰度值，建议值：0.0, 1.0, 2.0, 5.0, 10.0, 16.0, 32.0, 64.0, 128.0, 253.0, 254.0, 255.0")]
        public int GrayValue { get; set; } = 255;
        [Description("填充方式（边界形式），可选值： ‘fill’, ‘margin’")]
        public PaintRegion_Type Type { get; set; } = PaintRegion_Type.fill;

        public IRegionExtractor RegionExtractor { get; set; }
    }
}