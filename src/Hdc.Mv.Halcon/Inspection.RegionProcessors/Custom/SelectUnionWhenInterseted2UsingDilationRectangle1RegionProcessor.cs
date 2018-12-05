using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectUnionWhenInterseted2UsingDilationRectangle1RegionProcessor :
        SelectUnionWhenInterseted2RegionProcessor
    {
        protected override HRegion DilationRegion(HRegion region)
        {
            var dilation = region.DilationRectangle1(DilationRectangle1Width, DilationRectangle1Height);
            return dilation;
        }

        public int DilationRectangle1Width { get; set; }
        public int DilationRectangle1Height { get; set; }
    }
}