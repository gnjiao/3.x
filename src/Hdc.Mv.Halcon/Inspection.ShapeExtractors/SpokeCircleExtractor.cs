using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SpokeCircleExtractor : ICircleExtractor
    {
        public Circle FindCircle(HImage image, double centerX, double centerY, double innerRadius, double outerRadius)
        {
            Circle foundCircle = new Circle();
            double roundless;

            bool isOK;
            try
            {
                isOK = HDevelopExport.Singletone.ExtractCircle(
                    image,
                    centerX,
                    centerY,
                    innerRadius,
                    outerRadius,
                    out foundCircle, out roundless,
                    RegionsCount,
                    RegionWidth,
                    Sigma,
                    Threshold,
                    SelectionMode,
                    Transition,
                    Direct, EllipseMode, EllipseAlgorithm
                    );
            }
            catch (Exception)
            {
                isOK = false;
            }

            if (isOK)
            {
                return new Circle(
                    foundCircle.CenterX,
                    foundCircle.CenterY,
                    foundCircle.Radius);
                //                circleSearchingResult.Roundness = roundless;
            }
            else
            {
                return new Circle();
            }
        }

        public string Name { get; set; }

        public bool SaveCacheImageEnabled { get; set; }

        // 
        public int RegionsCount { get; set; }
        //        public int Hal_RegionHeight { get; set; }
        public int RegionWidth { get; set; }
        public double Sigma { get; set; }
        public double Threshold { get; set; }
        public SelectionMode SelectionMode { get; set; }
        public Transition Transition { get; set; }
        public CircleDirect Direct { get; set; }

        /// <summary>
        /// Circle, EllipseDiameterMin, EllipseDiameterMax
        /// </summary>
        public string EllipseMode { get; set; } = "Circle";

        /// <summary>
        /// List of values: 'fhuber', 'fitzgibbon', 'focpoints', 'fphuber', 'fptukey', 'ftukey', 'geohuber', 'geometric', 'geotukey', 'voss'
        /// </summary>
        public string EllipseAlgorithm { get; set; } = "fitzgibbon";
    }
}