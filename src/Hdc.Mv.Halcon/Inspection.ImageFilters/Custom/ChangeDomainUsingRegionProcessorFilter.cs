using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionProcessor")]
    public class ChangeDomainUsingRegionProcessorFilter: ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var domain = image.GetDomain();
            var region = RegionProcessor.Process(domain);
            var reducedImage = image.ChangeDomain(region);

            domain.Dispose();
            region.Dispose();

            return reducedImage;
        }

        public IRegionProcessor RegionProcessor { get; set; }
    }
}