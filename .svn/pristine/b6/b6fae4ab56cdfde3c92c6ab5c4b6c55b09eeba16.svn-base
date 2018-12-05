using System;
using System.Windows;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class PointDefinition : DefinitionBase
    {
        public double ActualX { get; set; }
        public double ActualY { get; set; }

        public Point ActualPoint
        {
            get { return new Point(ActualX, ActualY); }
            set
            {
                ActualX = value.X;
                ActualY = value.Y;
            }
        }

        public double RelativeX { get; set; }
        public double RelativeY { get; set; }

        public Point RelativePoint
        {
            get { return new Point(RelativeX, RelativeY); }
            set
            {
                RelativeX = value.X;
                RelativeY = value.Y;
            }
        }
    }
}