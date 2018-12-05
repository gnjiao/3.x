using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
//    [Serializable]
//    public class HoleToMeanFilter : IImageFilter
//    {
//        public HImage Process(HImage image)
//        {
//            var domain = image.GetDomain();
//            var fullDomainImage = image.FullDomain();
//            var fullDomain = fullDomainImage.GetDomain();
//
//            //
//            var expandImage = image.ExpandDomainGray(ExpansionRange);
//            var expandFullImage = expandImage.FullDomain();
//            expandFullImage.WriteImage("tiff", 0, "B:\\test-0-expandFullImage.tif");
//
//            //
//            var holeRegion = fullDomain.Difference(domain);
//            var connectedHoleRegion = holeRegion.Connection();
//            var meanRegion = RegionProcessor.Process(connectedHoleRegion);
//            var meanImage = image.ChangeDomain(meanRegion);
//            var meanedImage = ImageFilter.Process(meanImage);
//            meanedImage.WriteImage("tiff", 0, "B:\\test-1-meanedImage.tif");
//            //
//            var paintedImage = meanedImage.PaintGray(expandFullImage);
//            paintedImage.WriteImage("tiff", 0, "B:\\test-2-paintedImage.tif");
//
//            return paintedImage;
//        }
//
//        public int ExpansionRange { get; set; }
//        public IRegionProcessor RegionProcessor { get; set; }
//        public IImageFilter ImageFilter { get; set; }
//    }
    [Serializable]
    [ContentProperty("Items")]
    public class HoleToMeanFilter : Collection<IImageFilter> , IImageFilter
    {
        public HImage Process(HImage image)
        {
            var domain = image.GetDomain();
            var fullDomainImage = image.FullDomain();
            var fullDomain = fullDomainImage.GetDomain();

            //
            var expandImage = image.ExpandDomainGray(ExpansionRange);
            var expandFullImage = expandImage.FullDomain();
//            expandFullImage.WriteImage("tiff", 0, "D:\\test-0-expandFullImage.tif");

            //
            var holeRegion = fullDomain.Difference(domain);

            HImage paintedImage = expandFullImage;

            for (int i = 0; i < Items.Count; i++)
            {
                var imageFilter = Items[i];
                var reducedImage = image.ChangeDomain(holeRegion);
                var meanedImage = imageFilter.Process(reducedImage);
//                meanedImage.WriteImage("tiff", 0, "D:\\test-1-meanedImage_" + i + ".tif");

                var paintedImage2 = meanedImage.PaintGray(paintedImage);
                paintedImage.Dispose();
                paintedImage = paintedImage2;
//                paintedImage.WriteImage("tiff", 0, "D:\\test-2-paintedImage_" + i + ".tif");
            }

            return paintedImage;
        }

        [DefaultValue(2)]
        [Browsable(true)] //yx
        [Description(
            "Radius of the gray value expansion, measured in pixels, Suggested values: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 16"
            )]
        public int ExpansionRange { get; set; } = 2;
    }
}