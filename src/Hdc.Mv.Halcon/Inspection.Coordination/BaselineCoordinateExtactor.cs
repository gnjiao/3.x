using System;
using System.Windows;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class BaselineCoordinateExtactor : ICoordinateExtactor
    {
        public string Name { get; set; }

        public string CoordinateName { get; set; }

        public IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter)
        {
            var inputCoord = coordinate as RelativeCoordinate;
            if (inputCoord == null) return coordinate;

            var origin = OriginExtractor
                .FindPoint(image, inputCoord, pixelCellSideLengthInMillimeter);

            var refPoint = ReferenceExtractor
                .FindPoint(image, inputCoord, pixelCellSideLengthInMillimeter);

            var xInPixel = OriginOffsetX.GetPixelValue(OriginOffsetUnitType,
                pixelCellSideLengthInMillimeter);

            var yInPixel = OriginOffsetY.GetPixelValue(OriginOffsetUnitType,
                pixelCellSideLengthInMillimeter);


//            var newCoord = new RelativeCoordinate(oldCoord.OriginVector, oldCoord.CoordinateVectorAngle);
//            newCoord.ChangeOriginOffsetUsingActual(new Vector(origin.X + xInPixel, origin.Y + yInPixel));

            var vector = refPoint - origin;
            var angleToXOfLine = vector.GetAngleFromX();
            var angleToXOfBaseline = angleToXOfLine + AngleOffset;

            var newCoord = new RelativeCoordinate(
                new Vector(origin.X + xInPixel, origin.Y + yInPixel),angleToXOfBaseline);
//            newCoord.CoordinateVectorAngle = angleToXOfBaseline;
            return newCoord;
        }

        public double OriginOffsetX { get; set; }
        public double OriginOffsetY { get; set; }
        public double AngleOffset { get; set; }
        public UnitType OriginOffsetUnitType { get; set; }

        public IPointInCoordinateExtractor OriginExtractor { get; set; }
        public IPointInCoordinateExtractor ReferenceExtractor { get; set; }
    }
}