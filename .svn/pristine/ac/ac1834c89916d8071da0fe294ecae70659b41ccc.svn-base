using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DistanceBetweenTwoPointsResult
    {
        public DistanceBetweenTwoPointsDefinition Definition { get; set; }

        public double Distance { get; set; }
        public double DistanceInXAxis { get; set; }
        public double DistanceInYAxis { get; set; }

        public double StartPointXPath { get; set; }
        public double StartPointYPath { get; set; }
        public double EndPointXPath   { get; set; }
        public double EndPointYPath   { get; set; }

        public double DistanceInWorld
            => Distance.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double DistanceInXAxisInWorld
            => DistanceInXAxis.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double DistanceInYAxisInWorld
            => DistanceInYAxis.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}