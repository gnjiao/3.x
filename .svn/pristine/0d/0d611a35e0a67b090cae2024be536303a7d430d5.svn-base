using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class LinesGaussXldExtractor : XldExtractorBase
    {
        protected override HXLD ExtractInner(HImage image)
        {
            var edges = image.LinesGauss(sigma: Sigma, low: Low, high: High, lightDark: LightDark.ToHalconString(),
                extractWidth: ExtractWidth, lineModel: LineModel, completeJunctions: CompleteJunctions);
            return edges;
        }

        public double Sigma { get; set; } = 1.5;

        public double Low { get; set; } = 3;

        public double High { get; set; } = 8;

        public LightDark LightDark { get; set; }

        public string ExtractWidth { get; set; } = "true";

        public string LineModel { get; set; } = "bar-shaped";

        public string CompleteJunctions { get; set; } = "true";
    }
}