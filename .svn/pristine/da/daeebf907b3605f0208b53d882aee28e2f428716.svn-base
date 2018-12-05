using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class GenContoursSkeletonXldExtractor : IXldExtractor
    {
        public HXLD Extract(HImage image)
        {
            var region = RegionExtractor.Extract(image);
            var hxldCont = region.GenContoursSkeletonXld(Length, Mode);
            return hxldCont;
        }

        public IRegionExtractor RegionExtractor { get; set; }

        public int Length { get; set; } = 1;

        public string Mode { get; set; } = "filter";
    }
}