using System;
using System.Windows;
using System.Windows.Markup;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty(nameof(DataCodeExtractor))]
    public class DataCodeSearchingDefinition: DefinitionBase
    {
        // General
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
        /// <summary>
        /// Half Width
        /// </summary>
        public double ROIHalfWidth { get; set; }

        public IRegionExtractor RegionExtractor { get; set; }

        public IDataCodeExtractor DataCodeExtractor { get; set; }

        public DataCodeSearchingDefinition()
        {
        }

        public DataCodeSearchingDefinition(Line line, double roiHalfWidth = 0)
            : this(line.X1, line.Y1, line.X2, line.Y2, roiHalfWidth)
        {
        }

        public DataCodeSearchingDefinition(double startX, double startY, double endX, double endY, double roiHalfWidth = 0)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            ROIHalfWidth = roiHalfWidth;
        }

        public DataCodeSearchingDefinition(Point p1, Point p2, double roiHalfWidth = 0) :
            this(p1.X, p1.Y, p2.X, p2.Y, roiHalfWidth)
        {
        }

        public Line Line => new Line(StartX, StartY, EndX, EndY);

        public Line RelativeLine { get; set; }
    }
}