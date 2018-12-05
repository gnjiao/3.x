using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class AggregateSelectShapeXldProcessor : IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            HTuple values;
            switch (Feature)
            {
                case ShapeFeature.ContLength:
                    values = xld.LengthXld();
                    break;
                case ShapeFeature.Circularity:
                    values = xld.CircularityXld();
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (values.Length == 0)
                return xld;

            HXLD foundXld;
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
                    foundXld = xld.SelectShapeXld(Feature.ToHalconString(), "and", max - 0.0000001, max + 0.0000001);
                    break;
                case AggregateType.Min:
                    double min = values.TupleMin();
                    foundXld = xld.SelectShapeXld(Feature.ToHalconString(), "and", min - 0.0000001, min + 0.0000001);
                    break;
                default:
                    throw new NotImplementedException();
            }
            var countOfAll = xld.CountObj();
            var countOfFound = foundXld.CountObj();
            return foundXld;
        }

        public AggregateType AggregateType { get; set; } = AggregateType.Max;

        public ShapeFeature Feature { get; set; } = ShapeFeature.Area;
    }
}