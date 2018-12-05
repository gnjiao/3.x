using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MoveAndUnionRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var moveRegion = region.MoveRegion(Y, X);
            var union = region.Union2(moveRegion);
            moveRegion.Dispose();

            return union;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}