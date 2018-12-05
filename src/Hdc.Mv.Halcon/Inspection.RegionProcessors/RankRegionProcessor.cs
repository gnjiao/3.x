using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class RankRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            return region.RankRegion(Width, Height, Number);
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Number { get; set; }
    }
}