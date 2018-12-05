using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionProcessor")]
    public class IntersectionRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var region2 = RegionProcessor.Process(region.Clone());
            var intersection = region.Intersection(region2);
            return intersection;
        }

        public IRegionProcessor RegionProcessor { get; set; }
    }
}