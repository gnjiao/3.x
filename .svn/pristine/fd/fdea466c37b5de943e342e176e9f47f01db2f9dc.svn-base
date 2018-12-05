using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectShapeEntry
    {
        public SelectShapeEntry()
        {
        }

        public SelectShapeEntry(ShapeFeature feature, double min, double max)
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