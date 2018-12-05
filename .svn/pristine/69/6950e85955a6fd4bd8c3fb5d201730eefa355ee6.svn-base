using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class UnionCotangentialContoursXldProcessor: IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            var cont = xld as HXLDCont;
            if (cont == null)
                return xld;

            var unionCont = cont.UnionCotangentialContoursXld(FitClippingLength, FitLength, MaxTangAngle, MaxDist,
                MaxDistPerp, MaxOverlap, Mode);

            return unionCont;
        }

        public string Mode { get; set; } = "attr_forget";

        public double MaxOverlap { get; set; } = 2.0;

        public double MaxDistPerp { get; set; } = 10.0;

        public double MaxDist { get; set; } = 25.0;

        public double MaxTangAngle { get; set; } = 0.78539816;

        public double FitLength { get; set; } = 30.0;

        public double FitClippingLength { get; set; } = 0.0;
    }
}