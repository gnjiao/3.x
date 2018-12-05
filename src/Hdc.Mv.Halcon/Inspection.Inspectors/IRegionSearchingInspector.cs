using System.Collections.Generic;
using HalconDotNet;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Inspection
{
    public interface IRegionSearchingInspector
    {
        RegionSearchingResult SearchRegion(HImage image, RegionSearchingDefinition definition);
    }
}