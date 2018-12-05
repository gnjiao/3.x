using System;
using Core;
using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    public static class HXLDExtensions
    {
        public static void WriteXldToDxf(this HXLD xld, string fileName)
        {
            var xldCont = xld as HXLDCont;
            if (xldCont != null)
            {
                xldCont.WriteContourXldDxf(fileName);
                return;
            }

            var xldPoly = xld as HXLDPoly;
            if (xldPoly != null)
            {
                xldPoly.WritePolygonXldDxf(fileName);
                return;
            }

            var msg = "XLD is not support writing to dxf";
            msg.WriteLineInConsoleAndDebug();
//            throw new InvalidOperationException(msg);
        }
    }
}