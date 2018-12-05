using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Core.Linq;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionSearchingDefinition")]
    public class GetSmallestHRectangle2OfRegionCoordinateExtactor : ICoordinateExtactor
    {
        public string Name { get; set; }

        public string CoordinateName { get; set; }

        public IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter)
        {
            var inspector3 = InspectorFactory.CreateRegionSearchingInspector();

            RegionSearchingDefinition.UpdateRelativeCoordinate(coordinate);

            var CoordinateRegions = inspector3.SearchRegion(image,
                RegionSearchingDefinition);

            var region = CoordinateRegions.Region;
            var rect2 = region.GetSmallestHRectangle2();

            Point originPoint = new Point();
            var line = rect2.GetRoiLineFromRectangle2Phi();

            switch (RegionCenter_Direction)
            {
                case Direction.Center:
                    originPoint = new Point(rect2.Column, rect2.Row);
                    break;
                case Direction.Left:
                    originPoint = line.X1 < line.X2 ? line.GetPoint1() : line.GetPoint2();
                    break;
                case Direction.Right:
                    originPoint = line.X1 > line.X2 ? line.GetPoint1() : line.GetPoint2();
                    break;
                case Direction.Top:
                    throw new NotImplementedException();
                    //                                    originPoint = line.Y1 > line.Y2 ? line.GetPoint1() : line.GetPoint2();
/*
                    break;
*/
                case Direction.Bottom:
                    throw new NotImplementedException();
                    //                                    originPoint = line.Y1 < line.Y2 ? line.GetPoint1() : line.GetPoint2();
/*
                    break;
*/
            }

            var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
                    originPoint.X, originPoint.Y, -rect2.Angle);
            if (_coordinate == null) throw new ArgumentNullException(nameof(_coordinate));
            return _coordinate;
        }

        public RegionSearchingDefinition RegionSearchingDefinition { get; set; }
        public Direction RegionCenter_Direction { get; set; } = Direction.Center;
    }
}