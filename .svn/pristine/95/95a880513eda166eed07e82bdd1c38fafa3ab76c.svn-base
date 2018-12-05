using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class UnionAdjacentContoursXldProcessor : IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            var cont = xld as HXLDCont;
            if (cont == null)
                return xld;

            var unionCont = cont.UnionAdjacentContoursXld(MaxDistAbs, MaxDistRel, Mode);

            return unionCont;
        }

        public double MaxDistAbs { get; set; } = 10.0;

        public double MaxDistRel { get; set; } = 1;

        public string Mode { get; set; } = "attr_keep";
    }
}