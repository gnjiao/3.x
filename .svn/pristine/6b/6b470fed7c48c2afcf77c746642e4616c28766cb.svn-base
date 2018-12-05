using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SmoothContoursXldProcessor : IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            var cont = xld as HXLDCont;
            if (cont == null)
                return xld;
            HXLDCont smoothCont = cont.SmoothContoursXld(NumRegrPoints);

            return smoothCont;
        }

        public int NumRegrPoints { get; set; } = 5;
    }
}