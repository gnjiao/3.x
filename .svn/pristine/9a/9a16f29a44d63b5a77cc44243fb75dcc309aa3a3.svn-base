using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SegmentContoursXldProcessor : IXldProcessor
    {
        public string Mode { get; set; } = "lines_circles";

        public int SmoothCont { get; set; } = 5;

        public double MaxLineDist1 { get; set; } = 4.0;

        public double MaxLineDist2 { get; set; } = 2.0;

        public HXLD Process(HXLD xld)
        {
            var cont = xld as HXLDCont;
            if (cont == null)
                return xld;

            var seg = cont.SegmentContoursXld(Mode, SmoothCont, MaxLineDist1, MaxLineDist2);
            return seg;
        }
    }
}