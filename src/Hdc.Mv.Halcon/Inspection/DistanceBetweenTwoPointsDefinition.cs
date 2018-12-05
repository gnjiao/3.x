using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DistanceBetweenTwoPointsDefinition : DefinitionBase
    {
        public string PointName1 { get; set; }
        public string PointName2 { get; set; }
        public double StandardDistance { get; set; }
        public double StandardDistanceInXAxis { get; set; }
        public double StandardDistanceInYAxis { get; set; }
        public bool   IsXldAndRadial { get; set; }

        public double StandardDistanceInWorld
            => StandardDistance.ToMillimeterFromPixel(PixelCellSideLengthInMillimeter);

        public double StandardDistanceInXAxisInWorld
            => StandardDistanceInXAxis.ToMillimeterFromPixel(PixelCellSideLengthInMillimeter);

        public double StandardDistanceInYAxisInWorld
            => StandardDistanceInYAxis.ToMillimeterFromPixel(PixelCellSideLengthInMillimeter);
    }
}