using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class CropDomainRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            if (Disabled)
            {
                var region = RegionExtractor.Extract(image);
                return region;
            }

            var domain = image.GetDomain();
            if (domain.Area < 0.000001)
            {
                var region = RegionExtractor.Extract(image);
                return region;
            }

            var offsetRow1 = domain.GetRow1();
            var offsetColumn1 = domain.GetColumn1();
            var croppedImage = image.CropDomain();

            var croppedDomain = croppedImage.GetDomain();
            var fullDomainImage = croppedImage.FullDomain();
            var fullDomain = fullDomainImage.GetDomain();
            var fixedCroppedDomain = fullDomain.Intersection(croppedDomain);
            var fixedFullDomainImage = fullDomainImage.ChangeDomain(fixedCroppedDomain);

            var croppedRegion = RegionExtractor.Extract(fixedFullDomainImage);

            var movedRegion = croppedRegion.MoveRegion(offsetRow1, offsetColumn1);

            croppedImage.Dispose();
            croppedRegion.Dispose();
            croppedDomain.Dispose();
            fullDomainImage.Dispose();
            fullDomain.Dispose();
            fixedCroppedDomain.Dispose();
            fixedFullDomainImage.Dispose();
            domain.Dispose();

            return movedRegion;
        }

        [Description("高斯平滑直方图因子, 建议值: 0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0")]
        public IRegionExtractor RegionExtractor { get; set; }

        public bool Disabled { get; set; }
    }
}