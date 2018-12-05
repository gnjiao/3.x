using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionProcessor")]
    public class GetDomainAndProcessRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = image.GetDomain();

            if (RegionProcessor == null)
            {
                return region;
            }

            var procRegion = RegionProcessor.Process(region);
            region.Dispose();
            return procRegion;
        }

        public IRegionProcessor RegionProcessor { get; set; }
    }
}