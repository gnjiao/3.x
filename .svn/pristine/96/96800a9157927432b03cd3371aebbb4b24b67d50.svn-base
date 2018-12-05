namespace Hdc.Mv.Inspection
{
    public class CircleSearchingResult
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public bool HasError { get; set; }

        public bool IsNotFound { get; set; }

        public Circle Circle { get; set; }

        public Circle RelativeCircle { get; set; }

        public double Roundness { get; set; }

        public CircleSearchingDefinition Definition { get; set; }

        public CircleSearchingResult()
        {
        }

        public CircleSearchingResult(int index)
        {
            Index = index;
        }

        public CircleSearchingResult(int index, CircleSearchingDefinition definition)
        {
            Index = index;
            Definition = definition;
        }

        public CircleSearchingResult(int index, Circle circle)
        {
            Index = index;
            Circle = circle;
        }

        public CircleSearchingResult(int index, CircleSearchingDefinition definition, Circle circle)
        {
            Index = index;
            Circle = circle;
            Definition = definition;
        }

        public CircleSearchingResult(string name)
        {
            Name = name;
        }

        public double Diameter => Circle?.Radius*2 ?? 0.0;
        public double DiameterInWorld => Diameter.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double RelativeCenterX => RelativeCircle.CenterX;
        public double RelativeCenterXInWorld => RelativeCenterX.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double RelativeCenterY => RelativeCircle.CenterY;
        public double RelativeCenterYInWorld => RelativeCenterY.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double CenterX => Circle?.CenterX ?? 0.0;
        public double CenterY => Circle?.CenterY ?? 0.0;
        public double CenterXInWorld => Circle.CenterX.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
        public double CenterYInWorld => Circle.CenterY.ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}