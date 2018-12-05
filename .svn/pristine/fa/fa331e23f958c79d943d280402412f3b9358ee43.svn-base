using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class DefectDefinition
    {
        public DefectDefinition()
        {
            References = new Collection<SurfacePartReference>();
        }

        public string Name { get; set; }
        public Line RoiActualLine { get; set; }
        public Line RoiRelativeLine { get; set; }
        public double RoiHalfWidth { get; set; }
        public bool Domain_SaveCacheImageEnabled { get; set; }

        public HRegion DefectRegion { get; set; }
        public bool SaveCacheImageEnabled { get; set; }

        public Collection<SurfacePartReference> References { get; set; }

//        public IImageFilter ImageFilter { get; set; }
        public IRegionExtractor RegionExtractor { get; set; }
        public IRegionExtractor RegionExtractorExcluded { get; set; }
        //        public IRegionProcessor RegionProcessor { get; set; }
        public IImageFilter DefectImageFilter { get; set; }
        public IRegionExtractor DefectRegionExtractor { get; set; }
        public IRegionExtractor DefectRegionExtractorExcluded { get; set; }
        public IRegionProcessor DefectRegionProcessor { get; set; }
        public bool PaintExcludedRegionEnabled { get; set; }
        public double PaintExcludedRegionGray { get; set; } // 0 - 255
        public double PixelCellSideLengthInMillimeter { get; set; }
        public double DefectArea { get; set; }
        public string Location { get; set; } = "Top";
    }
}