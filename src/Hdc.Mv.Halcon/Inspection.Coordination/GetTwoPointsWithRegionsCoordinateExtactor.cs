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
    [ContentProperty("RegionSearchingDefinition")]
    public class GetTwoPointsWithRegionsCoordinateExtactor : ICoordinateExtactor
    {
        public string Name { get; set; }
        public string CoordinateName { get; set; }

        public IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter)
        {
            var inspector3 = InspectorFactory.CreateRegionSearchingInspector();

            // RegionSearchingDefinition.UpdateRelativeCoordinate(coordinate);
            foreach (var RegionSearchingDefinition in Definitions)
            {
                RegionSearchingDefinition.UpdateRelativeCoordinate(coordinate);
            }
            var CoordinateRegions = inspector3.SearchRegions(image, Definitions);

            int num = CoordinateRegions.Count;
            Point[] pt = new Point[num];
            Point ptError = new Point(0, 0);
            foreach (var region in CoordinateRegions)
            {
                
                try
                {
                    pt[region.Index] = region.Region.GetCenterPoint();
                }
                catch (Exception)
                {
                    pt[region.Index] = ptError;
                }
                
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

            var unionregion = CoordinateRegions[0].Region.Union2(CoordinateRegions[1].Region);
            double xx, yy, phi, length1, length2;
            unionregion.SmallestRectangle2(out yy, out xx, out phi, out length1, out length2);
            //var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(xx, yy, phi);
            // var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
            // originPoint.X, originPoint.Y, CoordAngle);
            var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
            originPoint.X, originPoint.Y, coordinate.GetCoordinateAngle());
            if (_coordinate == null) throw new ArgumentNullException(nameof(_coordinate));
            return _coordinate;
        }
        public Collection<RegionSearchingDefinition> Definitions { get; set; } = new Collection<RegionSearchingDefinition>();
        public int PointNumber { get; set; } = 2;
        private double CoordAngle { get; set; }
    }
}

