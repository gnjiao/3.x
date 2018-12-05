using System;
using System.Collections.Generic;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public abstract class SelectUnionWhenInterseted2RegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var region0Dilation = DilationRegion(region[1]);
            var region1Dilation = DilationRegion(region[2]);

            var intersection = region0Dilation.Intersection(region1Dilation);

            List<HRegion> selectedRegions = new List<HRegion>();

            var connectedRegion1 = region[1].Connection().ToList();
            foreach (var blob in connectedRegion1)
            {
                var dilationBlob = DilationRegion(blob);
                var intersectionBlob = intersection.Intersection(dilationBlob);
                if (intersectionBlob.Area > 0)
                    selectedRegions.Add(blob);
            }

            var connectedRegion2 = region[2].Connection().ToList();
            foreach (var blob in connectedRegion2)
            {
                var dilationBlob = DilationRegion(blob);
                var intersectionBlob = intersection.Intersection(dilationBlob);
                if (intersectionBlob.Area > 0)
                    selectedRegions.Add(blob);
            }

            var unionBlob = selectedRegions.Union();

            return unionBlob;

            //            if (unionBlob.Area > 0)
            //            {
            //                var union = region[1].Union2(region[2]);
            //                return union;
            //            }
            //
            //            return intersection;
        }

        protected abstract HRegion DilationRegion(HRegion region);
    }
}