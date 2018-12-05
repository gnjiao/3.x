using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class RegionResult : IDisposable
    {
        public HRegion Region { get; set; }

        public HRegion Domain { get; set; }

        public string SurfaceGroupName { get; set; }

        public string SurfaceName { get; set; }

        public string RegionName { get; set; }

        public void Dispose()
        {
            Region?.Dispose();
            Domain?.Dispose();
        }
    }
}