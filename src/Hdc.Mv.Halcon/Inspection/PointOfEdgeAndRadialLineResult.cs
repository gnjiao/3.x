using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class PointOfEdgeAndRadialLineResult
    {
        public PointOfEdgeAndRadialLineDefinition Definition { get; set; }

        public bool HasError { get; set; }

        public int Index { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Distance { get; set; }

        public double XInWorld => X.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double YInWorld => Y.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double DistanceInWorld => Distance.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double ActualOriginX { get; set; }
        public double ActualOriginY { get; set; }

        public double RelativeOriginX { get; set; }
        public double RelativeOriginY { get; set; }

        public double RelativeOriginXInWorld => X.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double RelativeOriginYInWorld => Y.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}