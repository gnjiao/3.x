using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("ThresholdImageFilter")]
    public class SelectDarkAndLightSpotUsingDynThresholdWithSelectByAreaRegionExtractor :
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
                        new SelectShapeEntry(ShapeFeature.Area, LightBlobAreaMin, LightBlobAreaMax)
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
                        new SelectShapeEntry(ShapeFeature.Area, DarkBlobAreaMin, DarkBlobAreaMax)
                    }
                };

                this.DarkRegionProcessor = darkComp;
            }

            SelectIntersectionRegionProcessor = new SelectUnionWhenInterseted2UsingDilationCircleRegionProcessor()
            {
                DilationCircleRadius = IntersectionDilationCircleRadius,
            };

            return base.ExtractInner(image);
        }

        public double LightBlobAreaMin { get; set; }
        public double LightBlobAreaMax { get; set; } = double.MaxValue;
        public double DarkBlobAreaMin { get; set; }
        public double DarkBlobAreaMax { get; set; } = double.MaxValue;
        public double IntersectionDilationCircleRadius { get; set; } = 0.5;
    }
}