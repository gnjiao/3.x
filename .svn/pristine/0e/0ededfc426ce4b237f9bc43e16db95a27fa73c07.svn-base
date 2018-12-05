using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MoveAndDifferenceReverseRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var moveRegion = region.MoveRegion(Y, X);
            var difference = moveRegion.Difference(region);
            moveRegion.Dispose();

            return difference;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}