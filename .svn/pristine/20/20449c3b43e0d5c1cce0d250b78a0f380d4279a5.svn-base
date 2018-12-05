using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class IntersectionOfDilationCircleAndShapeTransRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var dilation = region.DilationCircle(DilationCircleRadius);
            var shapeTrans = region.ShapeTrans(ShapeTransType);
            var intersection = dilation.Intersection(shapeTrans);

            dilation.Dispose();
            shapeTrans.Dispose();

            return intersection;
        }

        public double DilationCircleRadius { get; set; } = 3.5;

        public string ShapeTransType { get; set; } = "convex";
    }
}