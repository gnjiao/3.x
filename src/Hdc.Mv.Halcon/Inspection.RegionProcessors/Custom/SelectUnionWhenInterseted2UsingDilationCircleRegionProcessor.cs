using System;
using System.Collections.Generic;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectUnionWhenInterseted2UsingDilationCircleRegionProcessor :
        SelectUnionWhenInterseted2RegionProcessor
    {
        protected override HRegion DilationRegion(HRegion region)
        {
            var dilation = region.DilationCircle(DilationCircleRadius);
            return dilation;
        }

        public double DilationCircleRadius { get; set; } = 0.5;
    }

/*
    [Serializable]
    public class SelectUnionWhenInterseted2UsingDilationCircleRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var region0Dilation = region[1].DilationCircle(DilationCircleRadius);
            var region1Dilation = region[2].DilationCircle(DilationCircleRadius);

            var intersection = region0Dilation.Intersection(region1Dilation);

            List<HRegion> selectedRegions = new List<HRegion>();
 
            var connectedRegion1 = region[1].Connection().ToList();
            foreach (var blob in connectedRegion1)
            {
                var dilationBlob = blob.DilationCircle(DilationCircleRadius);
                var intersectionBlob = intersection.Intersection(dilationBlob);
                if(intersectionBlob.Area>0)
                    selectedRegions.Add(intersectionBlob);
            }
 
            var connectedRegion2 = region[2].Connection().ToList();
            foreach (var blob in connectedRegion2)
            {
                var dilationBlob = blob.DilationCircle(DilationCircleRadius);
                var intersectionBlob = intersection.Intersection(dilationBlob);
                if(intersectionBlob.Area>0)
                    selectedRegions.Add(intersectionBlob);
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

        public double DilationCircleRadius { get; set; } = 0.5;
    }*/
}