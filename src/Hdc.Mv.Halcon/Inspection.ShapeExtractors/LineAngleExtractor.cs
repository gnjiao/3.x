using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("LineExtractor")]
    public class LineAngleExtractor : IAngleExtractor
    {
        public double FindAngle(HImage image)
        {
            HImage inputImage = image.CopyImage();
            if (ImageFilter != null)
            {
                var hImage = ImageFilter.Process(inputImage);
                inputImage.Dispose();
                inputImage = hImage;
            }

            var line = LineExtractor.FindLine(inputImage, RoiLine);

            var vector = Reverse ? line.GetVectorFrom1To2() : line.GetVectorFrom2To1();

//            var vect2to1 = line.GetVectorFrom2To1();
            var angle = vector.GetAngleToX();

            if (LimitDegreeFrom0To360)
            {
                if (angle < 0)
                    return angle + 360.0;
            }

            if (SaveCacheImageEnabled)
            {
                var lineRegion = new HRegion();
                lineRegion.GenRegionLine(line.Y1, line.X1, line.Y2, line.X2);
                var paintImage = inputImage.PaintRegion(lineRegion, 240.0, "fill");
                paintImage.WriteImage("tiff", 0, SaveCacheImageFileName);
                paintImage.Dispose();
            }

            inputImage.Dispose();
            return angle;
        }

        public string Name { get; set; }

        public bool SaveCacheImageEnabled { get; set; }
        public string SaveCacheImageFileName { get; set; }

        public Line RoiLine { get; set; }

        public IImageFilter ImageFilter { get; set; }

        public ILineExtractor LineExtractor { get; set; }

        public bool Reverse { get; set; }

        public bool LimitDegreeFrom0To360 { get; set; }
    }
}