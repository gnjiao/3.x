using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Mvvm
{
    public class EllipseIndicatorViewModel
    {
        public double Row { get; set; }
        public double Column { get; set; }
        public double Phi { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }
        public string ColorName { get; set; }
        public int LineWidth { get; set; }
        public RegionFillMode RegionFillMode { get; set; }
    }
}
