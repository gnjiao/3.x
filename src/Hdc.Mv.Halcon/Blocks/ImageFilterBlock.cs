using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;
using Hdc.Mv.Inspection;
using Hdc.Mv.PropertyItem.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("ImageFilter", BlockCatagory.ImageProcessing)]
    public class ImageFilterBlock : RegionOfInterestBlock
    {
        [Browsable(false)]
        public Collection<HImage> Images { get; set; } = new BindingList<HImage>();

        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]        
        [NewItemTypes(
            typeof(MeanImageFilter),
            typeof(SobelAmpImageFilter),
            typeof(MedianImageFilter),
            typeof(MeanSpImageFilter),
            typeof(BinomialImageFilter),
            typeof(AdjustMeanImageFilter),
            typeof(AnisotropicDiffusionImageFilter),
            typeof(ChangeDomainImageFilter),
            typeof(CompositeImageFilter),
            typeof(ConvertImageTypeImageFilter),
            typeof(DomainToBinImageFilter),
            typeof(EdgesImageImageFilter),
            typeof(EmphasizeImageFilter),
            typeof(ExpandDomainGrayImageFilter),
            typeof(GrayClosingRectImageFilter),
            typeof(GrayClosingShapeImageFilter),
            typeof(GrayErosionRectImageFilter),
            typeof(GrayErosionShapeImageFilter),
            typeof(GrayOpeningRectImageFilter),
            typeof(GrayOpeningShapeImageFilter),
            typeof(GrayRangeRectImageFilter),
            typeof(HighpassImageFilter),
            typeof(InvertImageFilter),
            typeof(LaplaceOfGaussImageFilter),
            typeof(MedianRectImageFilter),
            typeof(MinMaxGrayScaleFilter),
            typeof(ReduceDomainFilter),
            typeof(ReduceDomainogImageFilter),
            typeof(RotateImageFilter),
            typeof(ScaleImageFilter),
            typeof(SubImage2Filter),
            typeof(SubImageFilter)

                )]   
        public List<IImageFilter> ImageFilters { get; set; } = new List<IImageFilter>();

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HImage InputImage { get; set; }

        [OutputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Output)] 
        public HImage OutputImage { get; set; }        

        public override void Process()
        {
            if (InputImage == null)
            {
                Status = BlockStatus.Error;
                Message = "InputImage is null";
                Exception = new BlockException("InputImage is null");
                return;
            }

            var image = InputImage;
            
            if(Roi?.RoiRegion != null)
                image = InputImage.ReduceDomain(Roi.RoiRegion);
            
            Images.Clear();
            Images.Add(InputImage);

            foreach (var imageFilter in ImageFilters)
            {
                var hImage = imageFilter.Process(image);
                Images.Add(hImage);

                image = hImage;
            }

            OutputImage = image;

            Status = BlockStatus.Valid;
            Message = "Process OK";
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
            imageViewer.Image = OutputImage;
        }
    }   
}