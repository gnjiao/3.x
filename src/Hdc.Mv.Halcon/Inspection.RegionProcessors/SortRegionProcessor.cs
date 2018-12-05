using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SortRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            return region.SortRegion(
                SortMode.ToHalconString(),
                Order.ToHalconString(),
                RowOrCol.ToHalconString());
        }

        public SortMode SortMode { get; set; }
        public Order Order { get; set; }
        public RowOrCol RowOrCol { get; set; }
    }
}