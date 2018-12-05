using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MoveAndSymmDifferenceRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var moveRegion = region.MoveRegion(Y, X);
            var difference = region.SymmDifference(moveRegion);
            moveRegion.Dispose();

            return difference;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}