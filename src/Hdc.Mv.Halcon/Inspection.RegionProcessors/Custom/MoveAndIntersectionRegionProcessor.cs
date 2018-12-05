using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MoveAndIntersectionRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var moveRegion = region.MoveRegion(Y, X);
            var intersection = region.Intersection(moveRegion);
            moveRegion.Dispose();

            return intersection;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}