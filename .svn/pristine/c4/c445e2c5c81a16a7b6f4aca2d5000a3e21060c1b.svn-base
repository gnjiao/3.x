using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Core.Linq;
using Hdc.Mv.Halcon;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("CircleSearchingDefinition")]
    public class GetTwoPointsWithCircleCoordinateExtactor : ICoordinateExtactor
    {
        public string Name { get; set; }
        public string CoordinateName { get; set; }

        public IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter)
        {
            var inspector4 = InspectorFactory.CreateCircleInspector("Hal");

            // RegionSearchingDefinition.UpdateRelativeCoordinate(coordinate);
            foreach (var circleSearchingDefinition in Definitions)
            {
                circleSearchingDefinition.UpdateRelativeCoordinate(coordinate);
            }
            var coordinateCircles = inspector4.SearchCircles(image, Definitions);

            int num = coordinateCircles.Count;
            Point[] pt = new Point[num];
            foreach (var cirlce in coordinateCircles)
            {
                pt[cirlce.Index].X = cirlce.CenterX;
                pt[cirlce.Index].Y = cirlce.CenterY;
            }
            Point originPoint = new Point();
            switch (PointNumber)
            {
                case 1:
                    originPoint.X = pt[0].X;
                    originPoint.Y = pt[0].Y;
                    break;
                case 2:
                    originPoint.X = (pt[0].X + pt[1].X) / 2;
                    originPoint.Y = (pt[0].Y + pt[1].Y) / 2;
                    if (pt[1].X >= pt[0].X)
                    {
                        Vector lineangle = new Vector(pt[1].X - pt[0].X, pt[1].Y - pt[0].Y);
                        CoordAngle = lineangle.GetAngleToX();
                    }
                    else
                    {
                        Vector lineangle = new Vector(pt[0].X - pt[1].X, pt[0].Y - pt[1].Y);
                        CoordAngle = lineangle.GetAngleToX();
                    }

                    break;
                case 3:
                    throw new NotImplementedException();
/*
                    break;
*/
                case 4:
                    throw new NotImplementedException();
                    //                                    originPoint = line.Y1 > line.Y2 ? line.GetPoint1() : line.GetPoint2();
/*
                    break;
*/
                case 5:
                    throw new NotImplementedException();
                    //                                    originPoint = line.Y1 < line.Y2 ? line.GetPoint1() : line.GetPoint2();
/*
                    break;
*/
            }

            //coordinate.GetCoordinateAngle();
            //默认两点连线为Y轴线，且从0到1为Y轴正向
            //AngleOffset若为正值则顺时针旋转
            CoordAngle = -90 - CoordAngle + AngleOffset;
            var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
                    originPoint.X, originPoint.Y, CoordAngle);
            if (_coordinate == null) throw new ArgumentNullException(nameof(_coordinate));
            return _coordinate;
        }
        public Collection<CircleSearchingDefinition> Definitions { get; set; } = new Collection<CircleSearchingDefinition>();
        public int PointNumber { get; set; } = 2;
        public double AngleOffset { get; set; } = 0;
        private double CoordAngle { get; set; }
    }
}

