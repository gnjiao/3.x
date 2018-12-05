using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class PartitionDynamicRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            return region.PartitionDynamic(Distance, Percent);
        }

        public double Distance { get; set; }
        public double Percent { get; set; }
    }
}