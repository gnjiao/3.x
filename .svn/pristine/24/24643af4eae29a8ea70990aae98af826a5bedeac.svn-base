using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class SurfacePartDefinition
    {
        private HRegion _domain;
        public string Name { get; set; }
        public bool SaveCacheImageEnabled { get; set; }
        public IRectangle2 RoiRelativeRect { get; set; }
        public IRectangle2 RoiActualRect { get; set; }
        public IRegionExtractor RegionExtractor { get; set; }

        public virtual HRegion Extract(HImage image)
        {
//            if (Domain == null && RoiActualRect == null)
//                _domain = image.GetDomain();

            if (RegionExtractor == null)
                return _domain;

            var domainRect = _domain.GetSmallestRectangle1();
            var domainCropImage = image.CropRectangle1(domainRect);

            var region = RegionExtractor.Extract(domainCropImage);
            var movedRegion = region.MoveRegion(domainRect.Row1, domainRect.Column1);
            return movedRegion;
        }

        [Obsolete]
        public virtual HRegion Extract(HImage image, HRegion domain)
        {
            var domainChangedImage = image.ChangeDomain(domain);

            if (RegionExtractor == null)
            {
                return domainChangedImage;
            }
            else
            {
                return RegionExtractor.Extract(domainChangedImage);
            }
        }

        public HRegion Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        public HRegion GetOrInitDomain(HImage image)
        {
            if (_domain == null && RoiActualRect == null)
            {
                _domain = image.GetDomain();
                return _domain;
            }

            if (_domain == null && RoiActualRect != null)
            {
                _domain = RoiActualRect.GenRegion();
                return _domain;
            }

            return _domain;
        }
    }
}