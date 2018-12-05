using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class RotateImageRegionExtractor: RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var image2 = image.CopyImage();
            
            var angle = AngleExtractor.FindAngle(image2);
            var phi = angle/180.0 * Math.PI;

            var mat2D = new HHomMat2D();
            var rotatedMat2D = mat2D.HomMat2dRotate(-phi, 0, 0);

            var reversedMat2D = mat2D.HomMat2dRotate(phi, 0, 0);

            var rotatedImage = image2.AffineTransImage(rotatedMat2D, Interpolation.ToHalconString(), "false");

//            if (SaveCacheImageEnabled)
//            {
//                rotatedImage.WriteImage("tiff", 0, SaveCacheImageFileName);
//            }

            var region = RegionExtractor.Extract(rotatedImage);

            var regionReversed = region.AffineTransRegion(reversedMat2D, "nearest_neighbor");

            rotatedImage.Dispose();
            region.Dispose();
            image2.Dispose();
           
            return regionReversed;
        }

        
        public IAngleExtractor AngleExtractor { get; set; }

        [Description("插值类型，可选值有： 'bicubic', 'bilinear', 'constant', 'nearest_neighbor', 'weighted'")]
        public Interpolation Interpolation { get; set; } = Interpolation.Bilinear;

        public IRegionExtractor RegionExtractor { get; set; }
    }
}