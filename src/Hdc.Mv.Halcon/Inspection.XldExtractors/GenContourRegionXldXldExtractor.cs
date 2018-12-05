using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class GenContourRegionXldXldExtractor: XldExtractorBase
    {
        protected override HXLD ExtractInner(HImage image)
        {
            var region = RegionExtractor.Extract(image);
            var contours = region.GenContourRegionXld(Mode);
            return contours;
        }

        public IRegionExtractor RegionExtractor { get; set; }

        /// <summary>
        /// 'border', 'border_holes', 'center'
        /// </summary>
        public string Mode { get; set; } = "border";
    }
}