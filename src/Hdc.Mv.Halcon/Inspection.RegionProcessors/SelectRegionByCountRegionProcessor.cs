using System;
using System.Collections.Generic;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectRegionByCountRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            if (MaxCount == 0)
                MaxCount = Int32.MaxValue;

            List<HRegion> selectedRegions = new List<HRegion>();

            var regionList = region.ToList();
            foreach (var hRegion in regionList)
            {
                var connectedRegion = hRegion.Connection();
                var count = connectedRegion.CountObj();

                if (count >= MinCount && count <= MaxCount)
                    selectedRegions.Add(hRegion);

                connectedRegion.Dispose();
            }

            if (selectedRegions.Count == 0)
            {
                var foundRegion = new HRegion();

                foundRegion.GenEmptyRegion();
                return foundRegion;
            }
            else
            {
                HRegion foundRegion = null;

                foreach (var hRegion in selectedRegions)
                {
                    if (foundRegion == null)
                    {
                        foundRegion = hRegion;
                        continue;
                    }
                    foundRegion.ConcatObj(hRegion);
                }

                return foundRegion;
            }
        }

        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}