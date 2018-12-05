using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class IntersectionMultiRegionProcessor : Collection<IRegionProcessor>, IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            HRegion intersection = region.Clone();
            foreach (var regionProcessor in Items)
            {
                var regionSub= regionProcessor.Process(region.Clone());
                intersection = intersection.Intersection(regionSub);
            }
            
            return intersection;
        }
    }
}