using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class Rect2PhiAngleExtractor : IAngleExtractor
    {
        public double FindAngle(HImage image)
        {
            var inputImage = image.CopyImage();

            var region = RegionExtractor.Extract(inputImage);
            var rect2 = region.GetSmallestHRectangle2();

            inputImage.Dispose();
            region.Dispose();

            return rect2.Angle;
        }

        public IRegionExtractor RegionExtractor { get; set; }
    }
}