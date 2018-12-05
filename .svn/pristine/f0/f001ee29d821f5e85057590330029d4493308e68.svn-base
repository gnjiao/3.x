using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class Intersection2RegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var region1 = RegionProcessor1.Process(region.Clone());
            var region2 = RegionProcessor2.Process(region.Clone());
            var intersection = region1.Intersection(region2);
            return intersection;
        }

        public IRegionProcessor RegionProcessor1 { get; set; }
        public IRegionProcessor RegionProcessor2 { get; set; }
    }
}