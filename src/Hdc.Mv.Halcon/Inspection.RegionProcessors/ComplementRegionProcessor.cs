using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ComplementRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var complementRegion = region.Complement();
            return complementRegion;
        }
    }
}