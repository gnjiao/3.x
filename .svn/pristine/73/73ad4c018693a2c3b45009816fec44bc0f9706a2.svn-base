using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ConvertToAverageWidthRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            if (region.Area == 0)
                return region;

            var area = region.GetArea();
            var width = region.GetWidth();
            var height = region.GetHeight();
            var smallestHRectangle2 = region.GetSmallestHRectangle2();
            var lineLenght = smallestHRectangle2.Length1 * 2;
            var avgWidth = (int)(area / lineLenght + 0.5);

            switch (BaseLinePosition)
            {
                case Direction.Left:
                    {
                        var movedRegion = region.MoveRegion(0, 1);
                        var edgeRegion = region.Difference(movedRegion);
                        var connection = edgeRegion.Connection();
                        var edgeRegion2 = connection.SelectShape("height", "and", lineLenght/2, height);
                        var movedEdge = edgeRegion2.MoveRegion(0, avgWidth - 1);
                        var union = edgeRegion2.Union2(movedEdge);
                        var closing = union.ClosingRectangle1(avgWidth, 1);
                        return closing;
                    }
                case Direction.Right:
                    {
                        var movedRegion = region.MoveRegion(0, -1);
                        var edgeRegion = region.Difference(movedRegion);
                        var connection = edgeRegion.Connection();
                        var edgeRegion2 = connection.SelectShape("height", "and", lineLenght / 2, height);
                        var movedEdge = edgeRegion2.MoveRegion(0, -avgWidth + 1);
                        var union = edgeRegion2.Union2(movedEdge);
                        var closing = union.ClosingRectangle1(avgWidth, 1);
                        return closing;
                    }
                case Direction.Top:
                    {
                        var movedRegion = region.MoveRegion(1, 0);
                        var edgeRegion = region.Difference(movedRegion);
                        var connection = edgeRegion.Connection();
                        var edgeRegion2 = connection.SelectShape("width", "and", lineLenght / 2, width);
                        var movedEdge = edgeRegion2.MoveRegion(avgWidth - 1, 0);
                        var union = edgeRegion2.Union2(movedEdge);
                        var closing = union.ClosingRectangle1(1, avgWidth);
                        return closing;
                    }
                case Direction.Bottom:
                    {
                        var movedRegion = region.MoveRegion(-1, 0);
                        var edgeRegion = region.Difference(movedRegion);
                        var connection = edgeRegion.Connection();
                        var edgeRegion2 = connection.SelectShape("width", "and", lineLenght / 2, width);
                        var movedEdge = edgeRegion2.MoveRegion(-avgWidth + 1, 0);
                        var union = edgeRegion2.Union2(movedEdge);
                        var closing = union.ClosingRectangle1(1, avgWidth);
                        return closing;
                    }
                default:
                    throw new NotSupportedException();
            }
        }

        public Direction BaseLinePosition { get; set; }

    }
}