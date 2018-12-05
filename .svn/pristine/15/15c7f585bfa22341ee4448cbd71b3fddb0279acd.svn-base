using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class FillUpShapeRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            return region.FillUpShape(Feature.ToHalconString(), Min, Max);
        }

        public ShapeFeature Feature { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}