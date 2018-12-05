using System;
using System.Windows;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class IntersectionCoordinateExtactor : ICoordinateExtactor
    {
        public string Name { get; set; }

        public string CoordinateName { get; set; }

        public IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter)
        {
            var inputCoord = coordinate as RelativeCoordinate;
            if (inputCoord == null) return coordinate;

            var xInPixel = OriginOffsetX.GetPixelValue(OriginOffsetUnitType,
                pixelCellSideLengthInMillimeter);

            var yInPixel = OriginOffsetY.GetPixelValue(OriginOffsetUnitType,
                pixelCellSideLengthInMillimeter);

            // extract lines
            var line1 = PrimaryLineExtractor.FindLine(image, inputCoord, pixelCellSideLengthInMillimeter);
            var line2 = SecondaryLineExtractor.FindLine(image, inputCoord, pixelCellSideLengthInMillimeter);

            var intersection = line1.IntersectionWith(line2);

            PrimaryLine = line1;
            SecondaryLine = line2;
            Intersection = intersection;

            // check if lines is parallel, intersection is (0,0)
            if (Math.Abs(intersection.X) < 0.000001 && Math.Abs(intersection.Y) < 0.000001)
            {
                return inputCoord;
            }

            //
            var relativePoint = inputCoord.GetRelativeVector(intersection.ToVector());
            var movedRelativePoint = relativePoint + new Vector(xInPixel, yInPixel);
            var actualPoint = inputCoord.GetOriginalVector(movedRelativePoint);

            if (UseInputCoordAngle)
            {
                // the new Coord use CoordinateVectorAngle of inputCoord

                var newCoord2 = new RelativeCoordinate(
                    new Vector(actualPoint.X, actualPoint.Y), inputCoord.CoordinateVectorAngle);
                return newCoord2;
            }
            else
            {
                // the new Coord use angle of line2

                var angleToXOfLine2InRad = HMisc.AngleLx(line2.Row1, line2.Column1, line2.Row2, line2.Column2);
                var angleToXOfLine2InDegree = angleToXOfLine2InRad * 180.0 / Math.PI;
                var angle = angleToXOfLine2InDegree < 0 ? angleToXOfLine2InDegree + 180 : angleToXOfLine2InDegree;
                var angleDiff = 90 - angle;

                var newCoord2 = new RelativeCoordinate(
                    new Vector(actualPoint.X, actualPoint.Y), angleDiff);
                return newCoord2;
            }
        }

        public double OriginOffsetX { get; set; }
        public double OriginOffsetY { get; set; }
        public UnitType OriginOffsetUnitType { get; set; }

        public ILineInCoordinateExtractor PrimaryLineExtractor { get; set; }
        public ILineInCoordinateExtractor SecondaryLineExtractor { get; set; }

        public Line PrimaryLine { get; private set; }
        public Line SecondaryLine { get; private set; }
        public Point Intersection { get; private set; }

        public bool UseInputCoordAngle { get; set; }
    }
}