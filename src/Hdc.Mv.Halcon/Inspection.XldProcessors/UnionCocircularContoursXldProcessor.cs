using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class UnionCocircularContoursXldProcessor : IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            var cont = xld as HXLDCont;
            if (cont == null)
                return xld;

            var unionCont = cont.UnionCocircularContoursXld(MaxArcAngleDiff, MaxArcOverlap, MaxTangentAngle, MaxDist,
                MaxRadiusDiff, MaxCenterDist, MergeSmallContours, Iterations);

            return unionCont;
        }

        public double MaxArcAngleDiff { get; set; } = 0.5;
        public double MaxArcOverlap { get; set; } = 0.1;
        public double MaxTangentAngle { get; set; } = 0.2;
        public double MaxDist { get; set; } = 30;
        public double MaxRadiusDiff { get; set; } = 10;
        public double MaxCenterDist { get; set; } = 10;
        public string MergeSmallContours { get; set; } = "true";
        public int Iterations { get; set; } = 1;
    }
}