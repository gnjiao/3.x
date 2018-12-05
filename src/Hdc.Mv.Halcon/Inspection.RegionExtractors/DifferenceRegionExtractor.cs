using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DifferenceRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var domain = image.GetDomain();
            HImage changeDomain1 = image.ChangeDomain(domain);
            var region1 = RegionExtractorInRegion.Extract(changeDomain1);
            HImage changeDomain2 = image.ChangeDomain(domain);
            var region2 = RegionExtractorInSub.Extract(changeDomain2);

            var unionRegion = region1.Difference(region2);

            changeDomain1.Dispose();
            changeDomain2.Dispose();
            region1.Dispose();
            region2.Dispose();

            return unionRegion;
        }

        public IRegionExtractor RegionExtractorInRegion { get; set; }
        public IRegionExtractor RegionExtractorInSub { get; set; }
    }
}