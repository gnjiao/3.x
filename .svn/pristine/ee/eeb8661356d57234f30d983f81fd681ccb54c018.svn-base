using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DistanceBetweenPointsOfXldAndRadialLineResult
    {
        public DistanceBetweenPointsOfXldAndRadialLineDefinition Definition { get; set; }
        public bool HasError { get; set; }
        public int Index { get; set; }


        public double X1 { get; set; }
        public double Y1 { get; set; }

        public double X2 { get; set; }
        public double Y2 { get; set; }

        public double Distance { get; set; }
        public double DistanceInWorld => Distance.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}