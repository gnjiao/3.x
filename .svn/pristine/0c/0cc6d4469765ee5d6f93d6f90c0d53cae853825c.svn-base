using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    public class RegionSearchingResult
    {
        public RegionSearchingDefinition Definition { get; set; }

        public HRegion Region { get; set; }

        public bool HasError { get; set; }

        public int Index { get; set; }

        public double Column1 => Region.GetColumn1();
        public double Column1InWorld => Column1.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Column1InWorldDouble => (Column1).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Column2 => Region.GetColumn2();
        public double Column2InWorld => Column2.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Column2InWorldDouble => (Column2).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Row1 => Region.GetRow1();
        public double Row1InWorld => Row1.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Row1InWorldDouble => (Row1).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Row2 => Region.GetRow2();
        public double Row2InWorld => Row2.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Row2InWorldDouble => (Row2).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Width => Region.GetWidth();
        public double WidthInWorld => Width.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double WidthInWorldDouble => (Width).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double Height => Region.GetHeight();
        public double HeightInWorld => Height.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double HeightInWorldDouble => (Height).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}