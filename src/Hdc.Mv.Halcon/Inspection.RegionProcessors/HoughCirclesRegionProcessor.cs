using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{

    [Serializable]
    public class HoughCirclesRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var centerRegion = region.HoughCircles(Radius, Percent, Mode);
            return centerRegion;
        }

        public int Radius { get; set; } = 12;

        public int Percent { get; set; } = 60;

        public int Mode { get; set; } = 0;
    }
}