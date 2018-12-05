using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionSearchingDefinition")]
    public class GetCenterOfRegionCoordinateExtactor : ICoordinateExtactor
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
            var originPoint = region.GetCenterPoint();

            var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
                originPoint.X, originPoint.Y, coordinate.GetCoordinateAngle());
            if (_coordinate == null) throw new ArgumentNullException(nameof(_coordinate));
            return _coordinate;
        }

        public RegionSearchingDefinition RegionSearchingDefinition { get; set; }
    }
}