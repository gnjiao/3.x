using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectHalfRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var rect1 =  region.GetSmallestRectangle1();
            switch (Direction)
            {
                case Direction.Top:
                    rect1.Row2 = rect1.Row1 + rect1.Height/2;
                    return rect1.ToHRegion();
                case Direction.Bottom:
                    rect1.Row1 = rect1.Row2 - rect1.Height/2;
                    return rect1.ToHRegion();
                case Direction.Left:
                    rect1.Column2 = rect1.Column1 + rect1.Width/2;
                    return rect1.ToHRegion();
                case Direction.Right:
                    rect1.Column1 = rect1.Column2 - rect1.Width/2;
                    return rect1.ToHRegion();
                default:
                    return region;
            }
        }

        public Direction Direction { get; set; }
    }
}