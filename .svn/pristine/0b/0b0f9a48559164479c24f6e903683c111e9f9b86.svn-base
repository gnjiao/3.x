using System;
using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    public static class HXLDContExtensions
    {
        public static Line FitLineContourXld(this HXLDCont hxldCont)
        {
            double beginRow, beginCol, endRow, endCol;
            double nr, nc, dist;
            hxldCont.FitLineContourXld("tukey", -1, 0, 5, 2, out beginRow, out beginCol, out endRow, out endCol, out nr,
                out nc, out dist);

            var line = new Line(beginCol, beginRow, endCol, endRow);
            return line;
        }

        public static Ellipse FitEllipseContourXld(this HXLDCont hxldCont)
        {
            double row, col, phi, rad1, rad2, startPhi, endPhi;
            string pointOrder;
            try
            {
                hxldCont.FitEllipseContourXld("fitzgibbon", -1, 0.0, 0, 200, 3, 2.0,
                    out row, out col, out phi, out rad1, out rad2, out startPhi, out endPhi, out pointOrder);

                return new Ellipse()
                {
                    Row = row,
                    Column = col,
                    Phi = phi,
                    Radius1 = rad1,
                    Radius2 = rad2,
                    StartPhi = startPhi,
                    EndPhi = endPhi,
                    PointOrder = pointOrder,
                };
            }
            catch (Exception)
            {
                return new Ellipse();
            }
        }
    }
}