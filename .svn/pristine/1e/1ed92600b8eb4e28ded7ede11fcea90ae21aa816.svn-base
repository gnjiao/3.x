using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public interface ICoordinateExtactor
    {
        string Name { get; }

        string CoordinateName { get; }

        IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter);
    }
}