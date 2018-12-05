using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class EdgesSubPixXldExtractor : XldExtractorBase
    {
        protected override HXLD ExtractInner(HImage image)
        {
            var edges = image.EdgesSubPix(filter: Filter, alpha: Alpha, low: Low, high: High);
            return edges;
        }

        public string Filter { get; set; } = "canny";

        public double Alpha { get; set; } = 1.0;

        public int Low { get; set; } = 20;

        public int High { get; set; } = 40;
    }
}