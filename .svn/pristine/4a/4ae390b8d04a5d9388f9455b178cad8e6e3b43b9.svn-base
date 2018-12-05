using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Inspection;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("RegionExtractor", BlockCatagory.RegionExtractor)]
    [ContentProperty("RegionExtractor")]
    public class RegionExtractorBlock: RegionOfInterestBlock
    {
        public override void Process()
        {
            if (InputImage == null)
            {
                Status = BlockStatus.Error;
                Message = "Image is null.";
                Exception = new BlockException("Image is null.");
                return;
            }

            //            if (RegionExtractor == null)
            //            {
            //                Status = BlockStatus.Error;
            //                Message = "RegionExtractor is null.";
            //                Exception = new BlockException("RegionExtractor is null.");
            //                return;
            //            }

            var image = InputImage;

            if (Roi?.RoiRegion != null)
                image = InputImage.ReduceDomain(Roi.RoiRegion);

            try
            {

                foreach (var RegionExtractor in RegionExtractors)
                {
                    Region = RegionExtractor.Extract(image);
                }

                Status = BlockStatus.Valid;
            }
            catch (Exception ex)
            {
                Status = BlockStatus.Error;
                Message = "RegionExtractorBlock Error! RegionExtractor.Extract() throw exception.";
                Exception = ex;
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
        }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HImage InputImage { get; set; }

        [OutputPort]
        [Category(BlockPropertyCategories.Output)]
        public HRegion Region { get; set; }

        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [NewItemTypes(
            typeof(ThresholdRegionExtractor),
            typeof(AutoThresholdRegionExtractor),
            typeof(BinaryThresholdDualRegionExtractor),
            typeof(BinaryThresholdRegionExtractor),
            typeof(CircleRegionExtractor),
            typeof(ConcatenateMultiplyRegionExtractor),
            typeof(CropDomainRegionExtractor),
            typeof(DifferenceRegionExtractor),
            typeof(DynThresholdCroppedRegionExtractor),
            typeof(DynThresholdRegionExtractor),
            typeof(GenRegionContourXldRegionExtractor),
            typeof(GetDomainRegionExtractor),
            typeof(HysteresisThresholdInvertRegionExtractor),
            typeof(HysteresisThresholdRegionExtractor),
            typeof(IntersectionRegionExtractor),
            typeof(ProcessDomainRegionExtractor),
            typeof(PolarTransRegionExtractor),
            typeof(ReferenceRegionExtractor),
            typeof(Rectangle2RegionExtractor),
            typeof(RegiongrowingRegionExtractor),
            typeof(RotateImageRegionExtractor),
            typeof(StructuredRegionExtractor),
            typeof(SymmDifferenceRegionExtractor),
            typeof(ThresholdByGrayMeanRegionExtractor),
            typeof(Union2RegionExtractor),
            typeof(UnionMultiplyRegionExtractor),
            typeof(ZoomImageRegionExtractor)

        )]
        public List<IRegionExtractor> RegionExtractors { get; set; } = new List<IRegionExtractor>();
        public IRegionExtractor RegionExtractor { get; set; }
    }
}