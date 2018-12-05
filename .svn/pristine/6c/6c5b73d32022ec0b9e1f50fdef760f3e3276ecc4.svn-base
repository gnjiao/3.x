using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class Rectangle2RegionExtractor : RegionExtractorBase, IRegionExtractor, IRectangle2
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = this.GenRegion();

            return region;
        }

        //        public Rectangle2 Rectangle2 { get; set; }

        [Description("矩形中心的X坐标")]
        public double X { get; set; }

        [Description("矩形中心的Y坐标")]
        public double Y { get; set; }

        [Description("倾斜角度")]
        public double Angle { get; set; }

        [Description("矩形宽度的一半尺寸")]
        public double HalfWidth { get; set; }

        [Description("矩形高度的一半尺寸")]
        public double HalfHeight { get; set; }
    }
}