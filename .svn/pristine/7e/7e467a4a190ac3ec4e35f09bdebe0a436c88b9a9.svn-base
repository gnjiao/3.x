using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class UnionMultiplyRegionExtractor : Collection<IRegionExtractor>, IRegionExtractor
    {
        public HRegion Extract(HImage image)
        {
            HRegion unionRegion = null;

            foreach (var regionProcessor in Items)
            {
                var domain = image.GetDomain();
                HImage changeDomain1 = image.ChangeDomain(domain);
                var subRegion = regionProcessor.Extract(changeDomain1);

                if (unionRegion == null)
                {
                    unionRegion = subRegion;
                    continue;
                }

                unionRegion = unionRegion.Union2(subRegion);
                changeDomain1.Dispose();
                subRegion.Dispose();
            }

            if (SaveCacheImageEnabled)
            {
                image.WriteImage("tiff", 0, SaveCacheImageFileName + ".ori.tif");
                var paintImage = image.PaintRegion(unionRegion, PaintGray, "fill");
                paintImage.WriteImage("tiff", 0, SaveCacheImageFileName + ".painted.tif");
                paintImage.Dispose();
            }

            return unionRegion;
        }

        public bool SaveCacheImageEnabled { get; set; }

        public string SaveCacheImageFileName { get; set; }

        public double PaintGray { get; set; } = 200.0;
    }
}