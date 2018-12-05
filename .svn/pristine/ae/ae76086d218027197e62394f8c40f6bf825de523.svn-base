using System;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("CircleExtractor")]
    public class CircleExtractorPointInCoordinateExtractor : IPointInCoordinateExtractor
    {
        public Point FindPoint(HImage image, IRelativeCoordinate coordinate, double pixelCellSideLengthInMillimeter)
        {
            if (CircleExtractor == null)
                return new Point();

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

            var relativeCenterXInPixel = RelativeCenterX.GetPixelValue(UnitType, pixelCellSideLengthInMillimeter);
            var relativeCenterYInPixel = RelativeCenterY.GetPixelValue(UnitType, pixelCellSideLengthInMillimeter);
            var innerRadiusInPixel = InnerRadius.GetPixelValue(UnitType, pixelCellSideLengthInMillimeter);
            var outerRadiusInPixel = OuterRadius.GetPixelValue(UnitType, pixelCellSideLengthInMillimeter);

            if (UseRelativeCenter)
            {
                var actualCenter = coordinate.GetOriginalVector(new Vector(relativeCenterXInPixel, relativeCenterYInPixel));
                ActualCenterX = actualCenter.X;
                ActualCenterY = actualCenter.Y;
            }

            var circle = CircleExtractor.FindCircle(roiImage, 
                ActualCenterX, ActualCenterY, innerRadiusInPixel,outerRadiusInPixel);

            roiImage.Dispose();

            var centerPoint = circle.GetCenterPoint();
            return centerPoint;
        }

        public Line RoiActualLine { get; set; }
        public Line RoiRelativeLine { get; set; }
        public UnitType UnitType { get; set; }
        public double RoiHalfWidth { get; set; }
        public bool SaveRoiImageEnabled { get; set; }
        public string SaveRoiImageFileName { get; set; }

        public bool UseRelativeCenter { get; set; }
        public double ActualCenterX { get; set; }
        public double ActualCenterY{ get; set; }
        public double RelativeCenterX { get; set; }
        public double RelativeCenterY { get; set; }
        public double InnerRadius { get; set; }
        public double OuterRadius { get; set; }

        public ICircleExtractor CircleExtractor { get; set; }
    }
}