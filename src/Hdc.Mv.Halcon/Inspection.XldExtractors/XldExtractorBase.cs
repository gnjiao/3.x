using System;
using System.IO;
using HalconDotNet;
using Hdc.Mv.Halcon;
using Core.Serialization;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public abstract class XldExtractorBase : IXldExtractor
    {
        public HXLD Extract(HImage image)
        {
            if (SaveCacheImageEnabled)
            {
                image.WriteImage("tiff", 0, SaveCacheImageFileName + "_0_Original.tif");
            }

            HXLD xld = ExtractInner(image);
//            var countXld = xld.CountObj();

            if (SaveCacheImageEnabled)
            {

                var originalImage = image.CropDomain();
                originalImage.WriteImage("tiff", 0, SaveCacheImageFileName + "_1_Original_CropDomain.tif");


                xld.WriteXldToDxf(SaveCacheImageFileName + "_2_XLD.dxf");

                //                var paintImage = image.PaintXld(xld, PaintGray);
                //                paintImage.WriteImage("tiff", 0, SaveCacheImageFileName + "_2_PaintXld.tif");
                //                var croppedImage = paintImage.CropDomain();
                //                croppedImage.WriteImage("tiff", 0, SaveCacheImageFileName + "_3_PaintXld_CropDomain.tif");
                //
                //                var region = ((HXLDCont)xld).GenRegionContourXld("margin");
                //                var paintRegionImage = image.PaintRegion(region, PaintGray, "fill");
                //                paintRegionImage.WriteImage("tiff", 0, SaveCacheImageFileName + "_4_PaintRegion_Margin.tif");

                originalImage.Dispose();
//                paintImage.Dispose();
//                paintRegionImage.Dispose();
//                croppedImage.Dispose();
            }

            return xld;
        }

        protected abstract HXLD ExtractInner(HImage image);

        public bool SaveCacheImageEnabled { get; set; }

        public string SaveCacheImageFileName { get; set; }

        public double PaintGray { get; set; } = 200.0;
    }
}