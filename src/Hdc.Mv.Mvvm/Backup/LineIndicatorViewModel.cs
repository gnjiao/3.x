using System.Windows.Media;

namespace Hdc.Mv.Mvvm
{
    public class LineIndicatorViewModel1
    {
        public double StartPointX { get; set; }
        public double StartPointY { get; set; }
        public double EndPointX { get; set; }
        public double EndPointY { get; set; }

        public LineIndicatorViewModel1()
        {
        }

        public LineIndicatorViewModel1(Line line)
        {
            StartPointX = line.X1;
            StartPointY = line.Y1;
            EndPointX = line.X2;
            EndPointY = line.Y2;
        }

        public bool DisplayEnabled { get; set; } = true;
    }
}