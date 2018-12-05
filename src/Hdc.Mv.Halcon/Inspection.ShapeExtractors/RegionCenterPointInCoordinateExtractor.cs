using System;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class RegionCenterPointInCoordinateExtractor : IPointInCoordinateExtractor
    {
        public Point FindPoint(HImage image, IRelativeCoordinate coordinate, double pixelCellSideLengthInMillimeter)
        {
            if (RegionExtractor == null)
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

            var region = RegionExtractor.Extract(roiImage);
            var centerPointOfRegion = region.GetCenterPoint();
//            var point1OfRegion = region.GetPoint1();
//            var centerPoint = centerPointOfRegion.ToVector() + point1OfRegion.ToVector();

            roiImage.Dispose();
            region.Dispose();

//            return centerPoint.ToPoint();
            return centerPointOfRegion;
        }

        public IRegionExtractor RegionExtractor { get; set; }
        public Line RoiActualLine { get; set; }
        public Line RoiRelativeLine { get; set; }
        public UnitType UnitType { get; set; }
        public double RoiHalfWidth { get; set; }
        public bool SaveRoiImageEnabled { get; set; }
        public string SaveRoiImageFileName { get; set; }
    }
}