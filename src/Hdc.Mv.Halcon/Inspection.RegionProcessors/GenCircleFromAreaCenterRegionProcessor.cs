using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GenCircleFromAreaCenterRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            double row, col;
            region.AreaCenter(out row, out col);
            HRegion circle = new HRegion();
            circle.GenCircle(row, col, Radius);
            return circle;
        }

        public double Radius { get; set; }
    }
}