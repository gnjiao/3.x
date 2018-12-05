using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MoveAndSymmDifferenceReverseRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var moveRegion = region.MoveRegion(Y, X);
            var difference = moveRegion.SymmDifference(region);
            moveRegion.Dispose();

            return difference;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}