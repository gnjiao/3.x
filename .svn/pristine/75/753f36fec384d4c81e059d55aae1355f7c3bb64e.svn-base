using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    public static class DefectResultExtensions
    {
        public static DefectResult ToDefectResult(this DefectInfo defectInfo)
        {
            var defectResult = new DefectResult()
            {
                Index = defectInfo.Index,
                TypeCode = defectInfo.TypeCode,
                X = defectInfo.X,
                Y = defectInfo.Y,
                Width = defectInfo.Width,
                Height = defectInfo.Height,
                Size = defectInfo.Size,
            };
            return defectResult;
        }

        public static DefectResult ToDefectResult(this HRegion region)
        {
            var center = region.GetSmallestRectangle1Center();
            var rect1 = region.GetSmallestRectangle1Region();

            var dr = new DefectResult()
            {
                //                Index = index,
                X = center.X,
                Y = center.Y,
                Width = region.GetWidth(),
                Height = region.GetHeight(),
                Size = region.GetArea(),
                //                Name = definition.Name,
                Region = region,
                SmallestRectangle1 = rect1,
            };

            return dr;
        }
    }
}