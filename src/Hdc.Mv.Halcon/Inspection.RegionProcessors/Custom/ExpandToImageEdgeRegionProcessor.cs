using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ExpandToImageEdgeRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            if (Direction == Direction.Left)
            {
                var width = region.GetWidth();
                var moveRegion = region.MoveRegion(0, -width);
                var union = region.Union2(moveRegion);
                var closing = union.ClosingRectangle1(width, 0);

                var column1 = region.GetColumn1();
                var row2 = region.GetRow2();
                var rect1 = new HRectangle1()
                {
                    Column2 = column1,
                    Row2 = row2,
                };

                var rect1Region = rect1.ToHRegion();
                var all = rect1Region.Union2(closing);
                return all;
            }

            throw new NotImplementedException();
        }

        public Direction Direction { get; set; }
    }
}