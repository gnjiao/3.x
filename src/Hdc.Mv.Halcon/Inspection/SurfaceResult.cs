using System;
using System.Collections.Generic;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SurfaceResult: IDisposable
    {
        public int Index { get; set; }

        public SurfaceDefinition Definition { get; set; }

        public HRegion IncludeRegion { get; set; }

        public IList<RegionResult> IncludeRegionResults { get; set; }

        public HRegion ExcludeRegion { get; set; }

        public IList<RegionResult> ExcludeRegionResults { get; set; }

        public SurfaceResult()
        {
            IncludeRegionResults = new List<RegionResult>();
            ExcludeRegionResults = new List<RegionResult>();
        }

        public HHomMat2D HomMat2D { get; set; }
        public TopLeftRectangle TopLeftRectangle { get; set; }
        public HImage TransformedImage { get; set; }

        public void Dispose()
        {
            TransformedImage.Dispose();
        }
    }
}