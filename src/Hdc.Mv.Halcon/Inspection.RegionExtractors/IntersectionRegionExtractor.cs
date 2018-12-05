using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class IntersectionRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var domain = image.GetDomain();
            HImage changeDomain1 = image.ChangeDomain(domain);
            var region1 = RegionExtractor1.Extract(changeDomain1);
            HImage changeDomain2 = image.ChangeDomain(domain);
            var region2 = RegionExtractor2.Extract(changeDomain2);

            var unionRegion = region1.Intersection(region2);

            changeDomain1.Dispose();
            changeDomain2.Dispose();
            region1.Dispose();
            region2.Dispose();

            return unionRegion;
        }

        public IRegionExtractor RegionExtractor1 { get; set; }
        public IRegionExtractor RegionExtractor2 { get; set; }
    }
}