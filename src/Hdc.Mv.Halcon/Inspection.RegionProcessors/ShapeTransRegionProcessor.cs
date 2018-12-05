using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ShapeTransRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            return region.ShapeTrans(Type.ToString());
        }

        public ShapeTransType Type { get; set; }
    }
}