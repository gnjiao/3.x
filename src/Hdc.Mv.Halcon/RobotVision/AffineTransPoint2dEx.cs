using System.Windows;
using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    public static class AffineTransPoint2dEx
    {
        public static Vector AffineTransPoint2d(this HHomMat2D mat2D, Vector vector)
        {
            HTuple x, y;
            x = mat2D.AffineTransPoint2d(vector.X, vector.Y, out y);
            return new Vector(x, y);
        }
    }
}