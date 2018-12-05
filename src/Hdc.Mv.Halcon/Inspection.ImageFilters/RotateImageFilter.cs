using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("AngleExtractor")]
    public class RotateImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            var angle = AngleExtractor.FindAngle(image);

            //            if (angle < 0)
            //                angle = angle%180;
//            angle += 180;

            var phi = angle / 180.0 * Math.PI;

            var mat2D = new HHomMat2D();
            var rotatedMat2D = mat2D.HomMat2dRotate(-phi, 0, 0);

            var reversedMat2D = mat2D.HomMat2dRotate(phi, 0, 0);

            var rotatedImage = image.AffineTransImage(rotatedMat2D, Interpolation.ToHalconString(), "false");

            return rotatedImage;
        }

        //[DefaultValue(90)]
        //[Browsable(true)]
        //[Description("Rotation angle")]

        [Description("旋转角度，建议值：90, 180, 270")]
        public IAngleExtractor AngleExtractor { get; set; } 

        //[DefaultValue("Bilinear")]
        //[Browsable(true)]
        //[Description("Type of interpolation, List of values: 'bilinear', 'constant', 'nearest_neighbor', 'weighted'")]
        [Description("插值类型，可选值有：‘bicubic’, ‘bilinear’, ‘constant’, ‘nearest_neighbor’, ‘weighted’")]
        public Interpolation Interpolation { get; set; } = Interpolation.Bilinear;
    }
}