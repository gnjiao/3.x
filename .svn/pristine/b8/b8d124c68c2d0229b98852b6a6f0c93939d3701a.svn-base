using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectShapeXldEntry
    {
        public SelectShapeXldEntry()
        {
        }

        public SelectShapeXldEntry(ShapeFeature feature, double min, double max)
        {
            Feature = feature;
            Min = min;
            Max = max;
        }

        public ShapeFeature Feature { get; set; } = ShapeFeature.Area;

        public double Min { get; set; }
        public double Max { get; set; }
    }
}