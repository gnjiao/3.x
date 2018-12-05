using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class ReduceDomainFilter : IImageFilter
    {
        public HImage Process(HImage image)
        {
            var region = RegionExtractor.Extract(image);
            var reducedImage = image.ReduceDomain(region);
            region.Dispose();
            return reducedImage;
        }

        public IRegionExtractor RegionExtractor { get; set; }
    }
}