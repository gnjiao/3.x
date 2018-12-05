using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    public interface IHomMat2DAndRectExtractor
    {
        void Extract(HImage image, out HHomMat2D homMat2D,  out TopLeftRectangle rectangle);

        Interpolation Interpolation { get; }
    }
}