using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    public class RegionTargetResult
    {
        public RegionTargetDefinition Definition { get; set; }

        public HRegion TargetRegion { get; set; }

        public bool HasError { get; set; }

        public int Index { get; set; }

        public double Column1 => TargetRegion.GetColumn1();
        public double Column1InWorld => Column1.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Column1InWorldDouble => (Column1).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Column2 => TargetRegion.GetColumn2();
        public double Column2InWorld => Column2.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Column2InWorldDouble => (Column2).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Row1 => TargetRegion.GetRow1();
        public double Row1InWorld => Row1.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Row1InWorldDouble => (Row1).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Row2 => TargetRegion.GetRow2();
        public double Row2InWorld => Row2.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Row2InWorldDouble => (Row2).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Width => TargetRegion.GetWidth();
        public double WidthInWorld => Width.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double WidthInWorldDouble => (Width).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Height => TargetRegion.GetHeight();
        public double HeightInWorld => Height.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double HeightInWorldDouble => (Height).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double VerticalHeight
        {
            get
            {
                var rect2 = TargetRegion.GetSmallestHRectangle2();
                var phiInDegree = rect2.Phi / Math.PI * 180.0;
                var reminder = phiInDegree % 180;
                if (Math.Abs(reminder) < 30)
                    return rect2.Length2 * 2;
                if (Math.Abs(reminder) > 150)
                    return rect2.Length1 * 2;
                return -999.999;
            }
        }
        public double VerticalHeightInWorld => VerticalHeight.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double HorizontalWidth
        {
            get
            {
                var rect2 = TargetRegion.GetSmallestHRectangle2();
                var phiInDegree = rect2.Phi / Math.PI * 180.0;
                var reminder = phiInDegree % 180;
                if (Math.Abs(reminder) < 30)
                    return rect2.Length1 * 2;
                if (Math.Abs(reminder) > 150)
                    return rect2.Length2 * 2;
                return -999.999;
            }
        }
        public double HorizontalWidthInWorld => HorizontalWidth.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}