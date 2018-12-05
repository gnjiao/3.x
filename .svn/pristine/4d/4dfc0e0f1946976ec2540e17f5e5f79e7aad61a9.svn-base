using System;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("ImageFilter")]
    public class LineInCoordinateExtractor : ILineInCoordinateExtractor
    {
        public IImageFilter ImageFilter { get; set; }
        public ILineExtractor LineExtractor { get; set; }
        public Line RoiActualLine { get; set; }
        public Line RoiRelativeLine { get; set; }
        public UnitType UnitType { get; set; }
        public double RoiHalfWidth { get; set; }
        public bool SaveRoiImageEnabled { get; set; }
        public string SaveRoiImageFileName { get; set; }

        public Line FindLine(HImage image, IRelativeCoordinate coordinate, double pixelCellSideLengthInMillimeter)
        {
            if (RoiRelativeLine != null && !RoiRelativeLine.IsEmpty)
            {
                var relativeLineInPixel = RoiRelativeLine.GetLineInPixel(UnitType, pixelCellSideLengthInMillimeter);
                var actualLine = relativeLineInPixel.UpdateRelativeCoordinate(coordinate);
                RoiActualLine = actualLine;
            }

            var halfWidthInPixel = RoiHalfWidth.GetPixelValue(UnitType, pixelCellSideLengthInMillimeter);

            var roiImage = HDevelopExport.Singletone.ChangeDomainForRectangle(
                image,
                RoiActualLine,
                halfWidthInPixel);

            if (SaveRoiImageEnabled)
            {
                roiImage.WriteImageOfTiffLzwOfCropDomain(SaveRoiImageFileName);
            }

            var filterImage = ImageFilter == null ? roiImage : ImageFilter.Process(roiImage);

            var line = LineExtractor.FindLine(filterImage, RoiActualLine);

            return line;
        }
    }
}