using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectObjRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            return region.SelectObj(Index);
        }

        public int Index { get; set; }
    }
}