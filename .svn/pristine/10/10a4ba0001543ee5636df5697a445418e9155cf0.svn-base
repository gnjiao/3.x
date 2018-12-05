using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ExtendLinesRegionProcessor : IRegionProcessor
    {
        public ExtendLinesRegionProcessor()
        {
            Iterations = 1;
        }

        public HRegion Process(HRegion region)
        {
            HRegion processRegion = region.MoveRegion(0, 0);

            for (int i = 0; i < Iterations; i++)
            {
                var connection = region.Connection();
                var selectedRegions = connection.SelectShape("rect2_len1", "and", Rect2Len1Min, Rect2Len1Max);
                var selectedRegionsUnion = selectedRegions.Union1();

                var dilationRegion = selectedRegionsUnion.DilationCircle(DilationCircleRadius);
                var union2 = dilationRegion.Union2(processRegion);

                var connection2 = union2.Connection();
                var selectedRegions2 = connection2.SelectShape("rect2_len1", "and", Rect2Len1Min, Rect2Len1Max);
                var selectedRegionsUnion2 = selectedRegions2.Union1();

                var intersection = processRegion.Intersection(selectedRegionsUnion2);
                var closed = intersection.ClosingCircle(DilationCircleRadius);
                processRegion = processRegion.Union2(closed);
            }

            return processRegion;
        }

        public double Rect2Len1Min { get; set; }
        public double Rect2Len1Max { get; set; }
        public double DilationCircleRadius { get; set; }
        public int Iterations { get; set; }
    }
}