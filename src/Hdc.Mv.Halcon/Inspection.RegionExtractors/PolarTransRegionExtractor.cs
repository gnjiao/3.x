using System;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class PolarTransRegionExtractor : RegionExtractorBase ,IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
//            image.WriteImage("tiff", 0, "B:\\test-01-cropped.tif");

            var imageWidth = image.GetWidth();
            var imageHeight = image.GetHeight();

//            image.GrayRangeRect(55, 55).WriteImage("tiff", 0, "B:\\test-00-GrayRangeRect.tif");

            var houghRegion = HoughRegionExtractor.Extract(image);
//            image.PaintRegion(houghRegion, 200.0, "fill").WriteImage("tiff", 0, "B:\\test-02-houghRegion.tif");

            var houghCenterRegion = houghRegion.HoughCircles(HoughExpectRadius, HoughPercent, 0);
            var center = houghCenterRegion.GetCenterPoint();

//            image.PaintRegion(houghCenterRegion, 200.0, "fill").WriteImage("tiff", 0, "B:\\test-03-houghCenterRegion.tif");

            var phiAngleStart = AngleStart/180.0*3.1415926;
            var phiAngleEnd = AngleEnd/180.0*3.1415926;

            var finalRadiuStart = RadiusStart > RadiusEnd ? RadiusStart : RadiusEnd;
            var finalRadiuEnd = RadiusStart > RadiusEnd ? RadiusEnd : RadiusStart;

            int width = (int) ((Math.Abs(phiAngleStart - phiAngleEnd))*finalRadiuStart);
            int height = (int) Math.Abs(RadiusStart - RadiusEnd);

            var transImage = image.PolarTransImageExt(center.Y, center.X, phiAngleStart, phiAngleEnd, RadiusStart,
                RadiusEnd,
                width, height, Interpolation.ToHalconString());

//            transImage.WriteImage("tiff", 0, "B:\\test-04-transImage.tif");

            var transRegion = TargetRegionExtractor.Extract(transImage);
            var finalRegion = transRegion.PolarTransRegionInv(center.Y, center.X, phiAngleStart, phiAngleEnd,
                RadiusStart, RadiusEnd,
                width, height, imageWidth, imageHeight, Interpolation.ToHalconString());

//            image.PaintRegion(finalRegion, 200.0, "fill").WriteImage("tiff", 0, "B:\\test-04-transRegion.tif");

            return finalRegion;
        }

        public IRegionExtractor HoughRegionExtractor { get; set; }
        public IRegionExtractor TargetRegionExtractor { get; set; }

        public double HoughExpectRadius { get; set; }
        public int HoughPercent { get; set; } = 60;

        [Description("区域在进行极坐标转换中，展开的起始角度，建议值： 0.0, 0.78539816, 1.57079632, 3.141592654, 6.2831853, 12.566370616")]
        public double AngleStart { get; set; } = 0;

        [Description("区域在进行极坐标转换中，展开的终止角度，建议值：0.0, 0.78539816, 1.57079632, 3.141592654, 6.2831853, 12.566370616")]
        public double AngleEnd { get; set; } = 360;

        [Description("区域在进行极坐标转换中，展开的起始半径，建议值：0, 16, 32, 64, 100, 128, 256, 512")]
        public double RadiusStart { get; set; }

        [Description("区域在进行极坐标转换中，展开的终止半径，建议值：0, 16, 32, 64, 100, 128, 256, 512")]
        public double RadiusEnd { get; set; }

        [Description("转变的插值方法，可选值有：'bilinear', 'nearest_neighbor', ' Constant','Weighted','Bicubic' ")]
        public Interpolation Interpolation { get; set; } = Interpolation.Bilinear;
    }
}