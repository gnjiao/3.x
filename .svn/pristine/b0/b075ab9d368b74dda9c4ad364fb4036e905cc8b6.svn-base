using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class AggregateSelectShapeRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var count = region.CountObj();
            if (count == 0)
                return region;

            HTuple values = region.RegionFeatures((HTuple) Feature.ToHalconString());

            HRegion foundRegion;
            switch (AggregateType)
            {
                case AggregateType.First:
                    throw new NotImplementedException();
                    //break;
                case AggregateType.Last:
                    throw new NotImplementedException();
                    //break;
                case AggregateType.Max:
                    double max = values.TupleMax();
                    foundRegion = region.SelectShape(Feature.ToHalconString(), "and", max - 0.0000001, max + 0.0000001);
                    break;
                case AggregateType.Min:
                    double min = values.TupleMin();
                    foundRegion = region.SelectShape(Feature.ToHalconString(), "and", min - 0.0000001, min + 0.0000001);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return foundRegion;
        }

        public AggregateType AggregateType { get; set; } = AggregateType.Max;

        public ShapeFeature Feature { get; set; } = ShapeFeature.Area;
    }
}