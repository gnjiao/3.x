using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class GenRectangle1RegionFromExistSmallestRectangle1RegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var smallest = region[1].GetSmallestRectangle1();
            var empty = new HRegion();
            switch (Direction)
            {
                case Direction.Left:
                    var col1L = smallest.Column1 - Width + HorizontalOffset;
                    var row1L = smallest.Row1 + VerticalOffset;
                    var col2L = col1L + Width;
                    var row2L = row1L + Height;
                    empty.GenRectangle1((double)row1L, col1L, row2L, col2L);
                    return empty;
                case Direction.Right:
                    var col1R = smallest.Column1 + smallest.Width + HorizontalOffset;
                    var row1R = smallest.Row1 + VerticalOffset;
                    var col2R = col1R + Width;
                    var row2R = row1R + Height;
                    empty.GenRectangle1((double)row1R, col1R, row2R, col2R);
                    return empty;
                case Direction.Top:
                    var col1T = smallest.Column1 + HorizontalOffset;
                    var row1T = smallest.Row1 - Height + VerticalOffset;
                    var col2T = col1T + Width;
                    var row2T = row1T + Height;
                    empty.GenRectangle1((double)row1T, col1T, row2T, col2T);
                    return empty;
                case Direction.Bottom:
                    var col1B = smallest.Column1 + HorizontalOffset;
                    var row1B = smallest.Row1 + smallest.Height + VerticalOffset;
                    var col2B = col1B + Width;
                    var row2B = row1B + Height;
                    empty.GenRectangle1((double)row1B, col1B, row2B, col2B);
                    return empty;
                default:
                    empty.GenEmptyRegion();
                    return empty;
            }
        }

        public Direction Direction { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int HorizontalOffset { get; set; }
        public int VerticalOffset { get; set; }
    }
}