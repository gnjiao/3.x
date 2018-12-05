using System;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ConvertImageTypeImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            return image.ConvertImageType(NewType.ToHalconString());
        }

        [DefaultValue(ImageType.Byte)]
        [Browsable(true)]
        [Description("需要图像的类型 (灰度类型).\n" +
                     "可选值为: 'byte', 'complex', 'cyclic', 'direction', 'int1', 'int2', 'int4', 'int8', 'real', 'uint2'")]
        public ImageType NewType { get; set; } = ImageType.Byte;
    }
}