using System;
using System.IO;
using HalconDotNet;
using Core.Serialization;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public abstract class RegionExtractorBase : IRegionExtractor
    {
        public HRegion Extract(HImage image)
        {
            if (SaveCacheImageEnabled)
            {
                image.WriteImage("tiff", 0, SaveCacheImageFileName + "_ori.tif");
            }

            HRegion region = null;

            region = ExtractInner(image);

            if (SaveCacheImageEnabled)
            {
                if (region.Area > 0)
                {
                    var paintImage = image.PaintRegion(region, PaintGray, "fill");
                    paintImage.WriteImage("tiff", 0, SaveCacheImageFileName + "_painted_fill.tif");
                    paintImage.Dispose();

                    var paintImageMargin = image.PaintRegion(region, PaintGray, "margin");
                    paintImageMargin.WriteImage("tiff", 0, SaveCacheImageFileName + "_painted_margin.tif");
                    paintImageMargin.Dispose();
                    var changedDomainImage = image.ChangeDomain(region);
                    var croppedImage = changedDomainImage.CropDomain();
                    croppedImage.WriteImage("tiff", 0, SaveCacheImageFileName + "_cropped.tif");
                    croppedImage.Dispose();
                    changedDomainImage.Dispose();
                }
                else
                {
                    image.WriteImage("tiff", 0, SaveCacheImageFileName + "_cropped(DomainIsEmpty).tif");
                }

            }

            return region;
        }

        protected abstract HRegion ExtractInner(HImage image);

        public bool SaveCacheImageEnabled { get; set; }

        public string SaveCacheImageFileName { get; set; }

        public double PaintGray { get; set; } = 200.0;
    }
}