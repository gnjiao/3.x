using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GetHoughCircleRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var centerRegion = region.HoughCircles(Radius, Percent, Mode);
            var centerX = centerRegion.GetCenterX();
            var centerY = centerRegion.GetCenterY();
            var circle = new HRegion();
            circle.GenCircle((double) centerY, centerX, Radius);
            return circle;
        }

        public int Radius { get; set; } = 12;

        public int Percent { get; set; } = 60;

        public int Mode { get; set; } = 0;
    }
}