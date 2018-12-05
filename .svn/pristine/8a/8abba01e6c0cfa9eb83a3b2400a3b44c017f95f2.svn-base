using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class ConcatenateMultiplyRegionExtractor : Collection<IRegionExtractor>, IRegionExtractor
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

                unionRegion = unionRegion.ConcatObj(subRegion);
                changeDomain1.Dispose();
//                subRegion.Dispose();
            }

            return unionRegion;
        }
    }
}