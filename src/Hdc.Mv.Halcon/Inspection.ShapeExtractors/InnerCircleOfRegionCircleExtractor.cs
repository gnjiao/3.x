using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class InnerCircleOfRegionCircleExtractor: ICircleExtractor
    {
        public Circle FindCircle(HImage image, double centerX, double centerY, double innerRadius, double outerRadius)
        {
            var region = RegionExtractor.Extract(image);
            double x, y, radius;
            region.InnerCircle(out y, out x, out radius);
            return new Circle(x, y, radius);
        }

        public string Name { get; set; }
        public bool SaveCacheImageEnabled { get; set; }

        public IRegionExtractor RegionExtractor { get; set; }
    }
}