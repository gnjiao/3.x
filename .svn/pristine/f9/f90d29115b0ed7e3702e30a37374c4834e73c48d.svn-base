using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("ThresholdImageFilter")]
    public class SelectDarkAndLightLineUsingDynThresholdRegionExtractor :
        SelectDarkAndLightRegionUsingDynThresholdRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            if (LightRegionProcessor == null)
            {
                var lightComp = new CompositeRegionProcessor
                {
                    new ConnectionRegionProcessor(),
                    new SelectShapeRegionProcessor
                    {
                        new SelectShapeEntry(ShapeFeature.Rect2Len1, LightBlobRect2Len1Min, LightBlobRect2Len1Max),
                        new SelectShapeEntry(ShapeFeature.Rect2Len2, LightBlobRect2Len2Min, LightBlobRect2Len2Max),
                    },
                };
                this.LightRegionProcessor = lightComp;
            }

            if (DarkRegionProcessor == null)
            {
                var darkComp = new CompositeRegionProcessor
                {
                    new ConnectionRegionProcessor(),
                    new SelectShapeRegionProcessor
                    {
                        new SelectShapeEntry(ShapeFeature.Rect2Len1, DarkBlobRect2Len1Min, DarkBlobRect2Len1Max),
                        new SelectShapeEntry(ShapeFeature.Rect2Len2, DarkBlobRect2Len2Min, DarkBlobRect2Len2Max),
                    }
                };

                this.DarkRegionProcessor = darkComp;
            }

            SelectIntersectionRegionProcessor = new SelectUnionWhenInterseted2UsingDilationRectangle1RegionProcessor()
            {
                DilationRectangle1Width = IntersectionDilationRectangle1Width,
                DilationRectangle1Height = IntersectionDilationRectangle1Height,
            };

            return base.ExtractInner(image);
        }

        public double LightBlobRect2Len1Min { get; set; }
        public double LightBlobRect2Len1Max { get; set; } = double.MaxValue;

        public double LightBlobRect2Len2Min { get; set; }
        public double LightBlobRect2Len2Max { get; set; } = double.MaxValue;

        public double DarkBlobRect2Len1Min { get; set; }
        public double DarkBlobRect2Len1Max { get; set; } = double.MaxValue;

        public double DarkBlobRect2Len2Min { get; set; }
        public double DarkBlobRect2Len2Max { get; set; } = double.MaxValue;

        public int IntersectionDilationRectangle1Width { get; set; }
        public int IntersectionDilationRectangle1Height { get; set; }
    }
}