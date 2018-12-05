using System;

namespace Hdc.Mv.Halcon
{
    public class HRectangle2
    {
        public double Row { get; set; }
        public double Column { get; set; }
        public double Phi { get; set; }
        public double Length1 { get; set; }
        public double Length2 { get; set; }

        public double Length => Length1 * 2;
        public double Width => Length2 * 2;

        public double HalfLength => Length1;
        public double HalfWidth => Length2;

        public double Angle => Phi / Math.PI * 180;
    }
}