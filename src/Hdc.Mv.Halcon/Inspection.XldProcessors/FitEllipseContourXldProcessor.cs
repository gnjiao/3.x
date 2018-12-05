using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class FitEllipseContourXldProcessor : IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            var cont = xld as HXLDCont;
            if (cont == null)
                return xld;

            var ellipse = cont.FitEllipseContourXld();

            if (ellipse.Radius1 < 0.000001 || ellipse.Radius2 < 0.000001)
                return xld;

            var ellipseXld = new HXLDCont();
            ellipseXld.GenEllipseContourXld(ellipse.Row, ellipse.Column, ellipse.Phi, ellipse.Radius1, ellipse.Radius2,
                ellipse.StartPhi, ellipse.EndPhi, ellipse.PointOrder, Resolution);

            return ellipseXld;
        }

        public double Resolution { get; set; } = 1.5;
    }
}