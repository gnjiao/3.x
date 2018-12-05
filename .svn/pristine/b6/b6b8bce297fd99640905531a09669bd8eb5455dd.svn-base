using System;
using System.Collections.Generic;
using System.Windows;

namespace Hdc.Mv
{
    [Serializable]
    public class ProjectionRectangle4
    {
        public Point TopLeftPoint { get; set; }
        public Point TopRightPoint { get; set; }
        public Point BottomLeftPoint { get; set; }
        public Point BottomRightPoint { get; set; }

        public IList<Point> GetPoints()
        {
//            var oldPoints = new List<Point>() { upperLeftPoint, upperRightPoint, lowerLeftPoint, lowerRightPoint };
            var oldPoints = new List<Point>() {TopLeftPoint, TopRightPoint, BottomLeftPoint, BottomRightPoint};

            return oldPoints;
        }

        public ProjectionRectangle4 GetHorizontalVertiacalRectangle4()
        {
            var newLeftColumn = (TopLeftPoint.X + BottomLeftPoint.X)/2.0;
            var newRightColumn = (TopRightPoint.X + BottomRightPoint.X)/2.0;
            var newUpperRow = (TopLeftPoint.Y + TopRightPoint.Y)/2.0;
            var newLowerRow = (BottomLeftPoint.Y + BottomRightPoint.Y)/2.0;

            var newUpperLeftPoint = new Point()
            {
                X = newLeftColumn,
                Y = newUpperRow,
            };

            var newUpperRightPoint = new Point()
            {
                X = newRightColumn,
                Y = newUpperRow,
            };

            var newLowerLeftPoint = new Point()
            {
                X = newLeftColumn,
                Y = newLowerRow,
            };

            var newLowerRightPoint = new Point()
            {
                X = newRightColumn,
                Y = newLowerRow,
            };

            return new ProjectionRectangle4()
            {
                TopLeftPoint = newUpperLeftPoint,
                TopRightPoint = newUpperRightPoint,
                BottomLeftPoint = newLowerLeftPoint,
                BottomRightPoint = newLowerRightPoint,
            };
        }

        public double TopCenterX => (TopLeftPoint.X + TopRightPoint.X)/2.0;
        public double TopCenterY => (TopLeftPoint.Y + TopRightPoint.Y)/2.0;
        public double BottomCenterX => (BottomLeftPoint.X + BottomRightPoint.X)/2.0;
        public double BottomCenterY => (BottomLeftPoint.Y + BottomRightPoint.Y)/2.0;
        public double LeftCenterX => (TopLeftPoint.X + BottomLeftPoint.X)/2.0;
        public double LeftCenterY => (TopLeftPoint.Y + BottomLeftPoint.Y)/2.0;
        public double RightCenterX => (TopRightPoint.X + BottomRightPoint.X)/2.0;
        public double RightCenterY => (TopRightPoint.Y + BottomRightPoint.Y)/2.0;

        public double CenterX => (TopCenterX + BottomCenterX)/2.0;
        public double CenterY => (LeftCenterY + RightCenterY)/2.0;
    }
}