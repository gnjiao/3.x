using System;
using System.Collections.Generic;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class RegionDefectResult : IDisposable
    {
        public RegionResult RegionResult { get; set; }

        public HRegion DefectRegion { get; set; }

        public IList<DefectResult> DefectResults { get; set; } = new List<DefectResult>();

        public double DefectArea { get; set; }

        public string Location { get; set; }

        public void Dispose()
        {
            DefectRegion?.Dispose();
            RegionResult?.Dispose();

            foreach (var defectResult in DefectResults)
            {
                defectResult.Dispose();
            }
        }
    }
}