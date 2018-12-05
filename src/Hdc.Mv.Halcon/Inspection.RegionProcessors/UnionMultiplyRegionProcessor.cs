using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class UnionMultiplyRegionProcessor : Collection<IRegionProcessor>, IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            if (Items.Count == 0)
                return region.MoveRegion(0,0);

            HRegion unionRegion = null;

            foreach (var regionProcessor in Items)
            {
                var subRegion = regionProcessor.Process(region.Clone());

                if (unionRegion == null)
                {
                    unionRegion = subRegion;
                    continue;
                }

                unionRegion = unionRegion.Union2(subRegion);
                subRegion.Dispose();
            }

            return unionRegion;
        }
    }
}