using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class IntersectionPointOfTwoShapesResult
    {
        public IntersectionPointOfTwoShapesDefinition Definition { get; set; }
        public bool HasError { get; set; }
        public int Index { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Distance { get; set; }

        public double XInWorld => X.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double YInWorld => Y.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double DistanceInWorld => Distance.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}