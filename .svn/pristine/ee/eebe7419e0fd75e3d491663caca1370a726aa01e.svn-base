using System;
using System.Windows;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class EdgeSearchingResult
    {
        public int Index { get; set; }

        public string Name
        {
            get { return Definition.Name; }
            set { Definition.Name = value; }
        }

        public bool HasError { get; set; }

        public bool IsNotFound { get; set; }

        public Line EdgeLine { get; set; }

        public Point IntersectionPoint { get; set; }

        public EdgeSearchingDefinition Definition { get; set; }

        public EdgeSearchingResult()
        {
        }

        public EdgeSearchingResult(int index)
        {
            Index = index;
        }

        public EdgeSearchingResult(int index, EdgeSearchingDefinition definition)
        {
            Index = index;
            Definition = definition;
        }

        public EdgeSearchingResult(string name)
        {
            Name = name;
        }

        public double X1 => EdgeLine.X1;
        public double X2 => EdgeLine.X2;
        public double Y1 => EdgeLine.Y1;
        public double Y2 => EdgeLine.Y2;

        public double X1InWorld => EdgeLine.X1.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double X2InWorld => EdgeLine.X2.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Y1InWorld => EdgeLine.Y1.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double Y2InWorld => EdgeLine.Y2.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double XMiddleInWorld => ((X1 + X2) / 2).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double YMiddleInWorld => ((Y1 + Y2) / 2).ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}