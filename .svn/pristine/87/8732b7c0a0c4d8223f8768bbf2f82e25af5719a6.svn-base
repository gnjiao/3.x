using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GetBorderBetweenErosionCirclesRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            if (region.Area == 0)
                return region;

            var outer = region.ErosionCircle(OuterErosionCircleRadius);
            if (outer.Area == 0)
                return region;

            var inner = region.ErosionCircle(InnerErosionCircleRadius);
            if (inner.Area == 0)
                return region;

            var diff = outer.Difference(inner);
            return diff;
        }

        public double OuterErosionCircleRadius { get; set; } = 5;
        public double InnerErosionCircleRadius { get; set; } = 10;
    }
}