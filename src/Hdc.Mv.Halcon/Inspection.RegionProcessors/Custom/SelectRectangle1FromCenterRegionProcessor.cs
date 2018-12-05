using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectRectangle1FromCenterRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var width = region.GetWidth();
            var height = region.GetHeight();
            var row = region.GetRow();
            var col = region.GetColumn();

            var finalWidth = Width == 0 ? width : Width;
            var finalHeight = Height == 0 ? height : Height;


            var row1 = row - finalHeight / 2 + OffsetY;
            var col1 = col - finalWidth / 2 + OffsetX;

            var row2 = row1 + finalHeight - 1;
            var col2 = col1 + finalWidth - 1;

            var rectRegion = new HRegion();
            rectRegion.GenRectangle1((HTuple)row1, col1, row2, col2);
            return rectRegion;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public int OffsetX { get; set; }

        public int OffsetY { get; set; }
    }
}