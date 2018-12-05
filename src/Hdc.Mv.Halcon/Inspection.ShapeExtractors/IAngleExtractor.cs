using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public interface IAngleExtractor
    {
        double FindAngle(HImage image);

//        string Name { get; set; }
//
//        bool SaveCacheImageEnabled { get; set; }
    }
}