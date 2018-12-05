using System.Windows;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public interface IPointInCoordinateExtractor
    {
        Point FindPoint(HImage image, IRelativeCoordinate coordinate, double pixelCellSideLengthInMillimeter);
    }
}